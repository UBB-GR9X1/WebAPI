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
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl = "http://localhost:5005/api";

        public RecommendationSystemProxy(HttpClient httpClient)
        {
            try
            {
                // Create a custom HttpClientHandler to bypass SSL certificate validation
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                
                // Create a new HttpClient with the custom handler
                _httpClient = new HttpClient(handler);
                Debug.WriteLine($"RecommendationSystemProxy initialized with base URL: {_baseApiUrl}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing RecommendationSystemProxy: {ex.Message}");
                throw;
            }
        }

        public async Task<List<RecommendationSystemDoctorModel>> GetDoctorsBySymptoms(string primarySymptom, string secondarySymptom, string tertiarySymptom, string discomfortArea, string symptomStart)
        {
            try
            {
                // For now, we'll get all doctors and filter them client-side
                Debug.WriteLine("Getting all doctors for symptom-based recommendation");
                var allDoctors = await GetAllDoctors();
                
                // Convert to the expected model type
                return allDoctors.Select(d => new RecommendationSystemDoctorModel
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

        public async Task<List<RecommendationSystemDoctorModel>> GetDoctorsByDepartmentPartialName(string departmentPartialName)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by department partial name: {departmentPartialName}");
                // Get all doctors and filter by department name
                var allDoctors = await GetAllDoctors();
                return allDoctors
                    .Where(d => d.GetDoctorDepartment()?.Contains(departmentPartialName, StringComparison.OrdinalIgnoreCase) ?? false)
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

        public async Task<List<RecommendationSystemDoctorModel>> GetDoctorsByPartialDoctorName(string doctorPartialName)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by partial name: {doctorPartialName}");
                // Get all doctors and filter by name
                var allDoctors = await GetAllDoctors();
                return allDoctors
                    .Where(d => d.DoctorName?.Contains(doctorPartialName, StringComparison.OrdinalIgnoreCase) ?? false)
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

        public async Task<List<RecommendationSystemDoctorJointModel>> GetDoctorsByDepartment(int departmentId)
        {
            try
            {
                Debug.WriteLine($"Getting doctors by department ID: {departmentId}");
                var response = await _httpClient.GetAsync($"{_baseApiUrl}/doctor/doctor/{departmentId}");
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
                var response = await _httpClient.GetAsync($"{_baseApiUrl}/doctor");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorJointModel>>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetAllDoctors: {ex.Message}");
                throw;
            }
        }

        public async Task<RecommendationSystemDoctorModel> GetDoctorById(int doctorId)
        {
            try
            {
                Debug.WriteLine($"Getting doctor by ID: {doctorId}");
                var response = await _httpClient.GetAsync($"{_baseApiUrl}/doctor/{doctorId}");
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

        public async Task<bool> UpdateDoctorName(int userId, string name)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/doctor/{userId}/name", name);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorEmail(int userId, string email)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/doctor/{userId}/email", email);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorCareerInfo(int userId, string careerInfo)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/doctor/{userId}/career-info", careerInfo);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorDepartment(int userId, int departmentId)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/doctor/{userId}/department", departmentId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorRating(int userId, double rating)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/doctor/{userId}/rating", rating);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorAvatarUrl(int userId, string newAvatarUrl)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/doctor/{userId}/avatar", newAvatarUrl);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDoctorPhoneNumber(int userId, string newPhoneNumber)
        {
            var response = await _httpClient.PutAsJsonAsync($"{_baseApiUrl}/doctor/{userId}/phone", newPhoneNumber);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLogService(int userId, ActionType type)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseApiUrl}/doctor/{userId}/log", type);
            return response.IsSuccessStatusCode;
        }
    }
}
