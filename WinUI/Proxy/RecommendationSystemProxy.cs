using ClassLibrary.Domain;
using ClassLibrary.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Repository;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace WinUI.Proxy
{
    internal class RecommendationSystemProxy : IRecommendationSystemDoctorRepository
    {
        private readonly HttpClient _http_client;
        private readonly string _base_api_url = "http://localhost:5005/api";

        public RecommendationSystemProxy(HttpClient http_client)
        {
            try
            {
                // Create a custom HttpClientHandler to bypass SSL certificate validation
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                
                // Create a new HttpClient with the custom handler
                this._http_client = new HttpClient(handler);
                Debug.WriteLine($"RecommendationSystemProxy initialized with base URL: {this._base_api_url}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing RecommendationSystemProxy: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorModel>> GetDoctorsBySymptoms(string primary_symptom, string secondary_symptom, string tertiary_symptom, string discomfort_area, string symptom_start)
        {
            try
            {
                // For now, we'll get all doctors and filter them client-side
                Debug.WriteLine("Getting all doctors for symptom-based recommendation");
                var all_doctors = await this.GetAllDoctors();
                
                // Convert to the expected model type
                return all_doctors.Select(d => new RecommendationSystemDoctorModel
                {
                    DoctorId = d.DoctorId,
                    DoctorName = d.DoctorName,
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.GetDoctorDepartment(),
                    Rating = d.Rating
                }).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetDoctorsBySymptoms: {ex.Message}");
                Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorModel>> GetDoctorsByDepartmentPartialName(string department_partial_name)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by department partial name: {department_partial_name}");
                // Get all doctors and filter by department name
                var all_doctors = await this.GetAllDoctors();
                return all_doctors
                    .Where(d => d.GetDoctorDepartment()?.Contains(department_partial_name, StringComparison.OrdinalIgnoreCase) ?? false)
                    .Select(d => new RecommendationSystemDoctorModel
                    {
                        DoctorId = d.DoctorId,
                        DoctorName = d.DoctorName,
                        DepartmentId = d.DepartmentId,
                        DepartmentName = d.GetDoctorDepartment(),
                        Rating = d.Rating
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetDoctorsByDepartmentPartialName: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorModel>> GetDoctorsByPartialDoctorName(string doctor_partial_name)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by partial name: {doctor_partial_name}");
                // Get all doctors and filter by name
                var all_doctors = await this.GetAllDoctors();
                return all_doctors
                    .Where(d => d.DoctorName?.Contains(doctor_partial_name, StringComparison.OrdinalIgnoreCase) ?? false)
                    .Select(d => new RecommendationSystemDoctorModel
                    {
                        DoctorId = d.DoctorId,
                        DoctorName = d.DoctorName,
                        DepartmentId = d.DepartmentId,
                        DepartmentName = d.GetDoctorDepartment(),
                        Rating = d.Rating
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetDoctorsByPartialDoctorName: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorJointModel>> GetDoctorsByDepartment(int department_id)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by department ID: {department_id}");
                var response = await this._http_client.GetAsync($"{this._base_api_url}/doctor/doctor/{department_id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorJointModel>>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetDoctorsByDepartment: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorJointModel>> GetAllDoctors()
        {
            try
            {
                Debug.WriteLine("Getting all doctors");
                var response = await this._http_client.GetAsync($"{this._base_api_url}/doctor");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorJointModel>>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetAllDoctors: {ex.Message}");
                throw;
            }
        }

        public async Task<RecommendationSystemDoctorModel> GetDoctorById(int doctor_id)
        {
            try
            {
                Debug.WriteLine($"Getting doctor by ID: {doctor_id}");
                var response = await this._http_client.GetAsync($"{this._base_api_url}/doctor/{doctor_id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                var doctor = await response.Content.ReadFromJsonAsync<RecommendationSystemDoctorJointModel>();
                if (doctor == null)
                    return null;

                return new RecommendationSystemDoctorModel
                {
                    DoctorId = doctor.DoctorId,
                    DoctorName = doctor.DoctorName,
                    DepartmentId = doctor.DepartmentId,
                    DepartmentName = doctor.GetDoctorDepartment(),
                    Rating = doctor.Rating
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetDoctorById: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateDoctorName(int user_id, string name)
        {
            var response = await this._http_client.PutAsJsonAsync($"{this._base_api_url}/doctor/{user_id}/name", name);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorEmail(int user_id, string email)
        {
            var response = await this._http_client.PutAsJsonAsync($"{this._base_api_url}/doctor/{user_id}/email", email);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorCareerInfo(int user_id, string career_info)
        {
            var response = await this._http_client.PutAsJsonAsync($"{this._base_api_url}/doctor/{user_id}/career-info", career_info);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorDepartment(int user_id, int department_id)
        {
            var response = await this._http_client.PutAsJsonAsync($"{this._base_api_url}/doctor/{user_id}/department", department_id);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorRating(int user_id, double rating)
        {
            var response = await this._http_client.PutAsJsonAsync($"{this._base_api_url}/doctor/{user_id}/rating", rating);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorAvatarUrl(int user_id, string new_avatar_url)
        {
            var response = await this._http_client.PutAsJsonAsync($"{this._base_api_url}/doctor/{user_id}/avatar", new_avatar_url);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorPhoneNumber(int user_id, string new_phone_number)
        {
            var response = await this._http_client.PutAsJsonAsync($"{this._base_api_url}/doctor/{user_id}/phone", new_phone_number);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLogService(int user_id, ActionType type)
        {
            var response = await this._http_client.PostAsJsonAsync($"{this._base_api_url}/doctor/{user_id}/log", type);
            return response.IsSuccessStatusCode;
        }
    }
}
