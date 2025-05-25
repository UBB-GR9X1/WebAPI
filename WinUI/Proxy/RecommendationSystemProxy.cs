using ClassLibrary.Domain;
using ClassLibrary.Repository;
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
    /// <summary>
    /// Provides proxy methods to interact with the Notification Web API.
    /// </summary>
    internal class RecommendationSystemProxy : IRecommendationSystemDoctorRepository
    {
        /// <summary>
        /// The HttpClient instance used to make HTTP requests.
        /// </summary>
        private readonly HttpClient _http_client;
        private readonly string s_base_api_url = Config._base_api_url;

        public RecommendationSystemProxy(HttpClient http_client)
        {
            try
            {
                // Create a custom HttpClientHandler to bypass SSL certificate validation
                HttpClientHandler handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                
                // Create a new HttpClient with the custom handler
                this._http_client = new HttpClient(handler);
                Debug.WriteLine($"RecommendationSystemProxy initialized with base URL: {this.s_base_api_url}");
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error initializing RecommendationSystemProxy: {exception.Message}");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<RecommendationSystemDoctorModel>> getDoctorsBySymptoms(string primary_symptom, string secondary_symptom, string tertiary_symptom, string discomfort_area, string symptom_start)
        {
            try
            {
                // For now, we'll get all doctors and filter them client-side
                Debug.WriteLine("Getting all doctors for symptom-based recommendation");
                List<RecommendationSystemDoctorJointModel> all_doctors = await this.getAllDoctors();
                
                // Convert to the expected model type
                return all_doctors.Select(doctor => new RecommendationSystemDoctorModel
                {
                    doctorId = doctor.doctorId,
                    doctorName = doctor.doctorName,
                    departmentId = doctor.departmentId,
                    departmentName = doctor.getDoctorDepartment(),
                    rating = doctor.rating
                }).ToList();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error in getDoctorsBySymptoms: {exception.Message}");
                Debug.WriteLine($"Inner Exception: {exception.InnerException?.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorModel>> getDoctorsByDepartmentPartialName(string department_partial_name)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by department partial name: {department_partial_name}");
                // Get all doctors and filter by department name
                List<RecommendationSystemDoctorJointModel> all_doctors = await this.getAllDoctors();
                return all_doctors
                    .Where(doctor => doctor.getDoctorDepartment()?.Contains(department_partial_name, StringComparison.OrdinalIgnoreCase) ?? false)
                    .Select(doctor => new RecommendationSystemDoctorModel
                    {
                        doctorId = doctor.doctorId,
                        doctorName = doctor.doctorName,
                        departmentId = doctor.departmentId,
                        departmentName = doctor.getDoctorDepartment(),
                        rating = doctor.rating
                    })
                    .ToList();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error in getDoctorsByDepartmentPartialName: {exception.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorModel>> getDoctorsByPartialDoctorName(string doctor_partial_name)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by partial name: {doctor_partial_name}");
                // Get all doctors and filter by name
                List<RecommendationSystemDoctorJointModel> all_doctors = await this.getAllDoctors();
                return all_doctors
                    .Where(doctor => doctor.doctorName?.Contains(doctor_partial_name, StringComparison.OrdinalIgnoreCase) ?? false)
                    .Select(doctor => new RecommendationSystemDoctorModel
                    {
                        doctorId = doctor.doctorId,
                        doctorName = doctor.doctorName,
                        departmentId = doctor.departmentId,
                        departmentName = doctor.getDoctorDepartment(),
                        rating = doctor.rating
                    })
                    .ToList();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error in getDoctorsByPartialDoctorName: {exception.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorJointModel>> getDoctorsByDepartment(int department_id)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by department ID: {department_id}");
                HttpResponseMessage response = await this._http_client.GetAsync($"{this.s_base_api_url}doctor/doctor/{department_id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorJointModel>>();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error in getDoctorsByDepartment: {exception.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorJointModel>> getAllDoctors()
        {
            try
            {
                Debug.WriteLine("Getting all doctors");
                HttpResponseMessage response = await this._http_client.GetAsync($"{this.s_base_api_url}doctor");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorJointModel>>();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error in getAllDoctors: {exception.Message}");
                throw;
            }
        }

        public async Task<RecommendationSystemDoctorModel> getDoctorById(int doctor_id)
        {
            try
            {
                Debug.WriteLine($"Getting doctor by ID: {doctor_id}");
                HttpResponseMessage response = await this._http_client.GetAsync($"{this.s_base_api_url}doctor/{doctor_id}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();
                var doctor = await response.Content.ReadFromJsonAsync<RecommendationSystemDoctorJointModel>();
                if (doctor == null)
                    return null;

                return new RecommendationSystemDoctorModel
                {
                    doctorId = doctor.doctorId,
                    doctorName = doctor.doctorName,
                    departmentId = doctor.departmentId,
                    departmentName = doctor.getDoctorDepartment(),
                    rating = doctor.rating
                };
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Error in getDoctorById: {exception.Message}");
                throw;
            }
        }

        public async Task<bool> updateDoctorName(int user_id, string name)
        {
            var response = await this._http_client.PutAsJsonAsync($"{this.s_base_api_url}doctor/{user_id}/name", name);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> updateDoctorEmail(int user_id, string email)
        {
            HttpResponseMessage response = await this._http_client.PutAsJsonAsync($"{this.s_base_api_url}doctor/{user_id}/email", email);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> updateDoctorCareerInfo(int user_id, string career_info)
        {
            HttpResponseMessage response = await this._http_client.PutAsJsonAsync($"{this.s_base_api_url}doctor/{user_id}/career-info", career_info);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> updateDoctorDepartment(int user_id, int department_id)
        {
            HttpResponseMessage response = await this._http_client.PutAsJsonAsync($"{this.s_base_api_url}doctor/{user_id}/department", department_id);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> updateDoctorRating(int user_id, double rating)
        {
            HttpResponseMessage response = await this._http_client.PutAsJsonAsync($"{this.s_base_api_url}doctor/{user_id}/rating", rating);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> updateDoctorAvatarUrl(int user_id, string new_avatar_url)
        {
            HttpResponseMessage response = await this._http_client.PutAsJsonAsync($"{this.s_base_api_url}doctor/{user_id}/avatar", new_avatar_url);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> updateDoctorPhoneNumber(int user_id, string new_phone_number)
        {
            HttpResponseMessage response = await this._http_client.PutAsJsonAsync($"{this.s_base_api_url}doctor/{user_id}/phone", new_phone_number);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> updateLogService(int user_id, ActionType type)
        {
            HttpResponseMessage response = await this._http_client.PostAsJsonAsync($"{this.s_base_api_url}doctor/{user_id}/log", type);
            return response.IsSuccessStatusCode;
        }
    }
}
