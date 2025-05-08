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
                var queryParams = new Dictionary<string, string>
                {
                    { "primarySymptom", primarySymptom },
                    { "secondarySymptom", secondarySymptom },
                    { "tertiarySymptom", tertiarySymptom },
                    { "discomfortArea", discomfortArea },
                    { "symptomStart", symptomStart }
                };

                var queryString = string.Join("&", queryParams.Select(x => $"{x.Key}={Uri.EscapeDataString(x.Value)}"));
                var url = $"{_baseApiUrl}/doctor/recommend?{queryString}";
                Debug.WriteLine($"Attempting to call API endpoint: {url}");

                var response = await _httpClient.GetAsync(url);
                Debug.WriteLine($"API Response Status: {response.StatusCode}");
                
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorModel>>();
                Debug.WriteLine($"Successfully retrieved {result?.Count ?? 0} doctors");
                return result;
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
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/doctor/department-partial?name={Uri.EscapeDataString(departmentPartialName)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorModel>>();
        }

        public async Task<List<RecommendationSystemDoctorModel>> GetDoctorsByPartialDoctorName(string doctorPartialName)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/doctor/name-partial?name={Uri.EscapeDataString(doctorPartialName)}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorModel>>();
        }

        public async Task<List<RecommendationSystemDoctorJointModel>> GetDoctorsByDepartment(int departmentId)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/doctor/department/{departmentId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorJointModel>>();
        }

        public async Task<List<RecommendationSystemDoctorJointModel>> GetAllDoctors()
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/doctor");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<RecommendationSystemDoctorJointModel>>();
        }

        public async Task<RecommendationSystemDoctorModel> GetDoctorById(int doctorId)
        {
            var response = await _httpClient.GetAsync($"{_baseApiUrl}/doctor/{doctorId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<RecommendationSystemDoctorModel>();
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
