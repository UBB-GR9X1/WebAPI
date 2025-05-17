using ClassLibrary.IRepository;
using ClassLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WinUI.Proxy
{
    public class DoctorsProxy : IDoctorRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5005/";

        public DoctorsProxy(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task AddDoctorAsync(Doctor doctor)
        {
            string doctorJson = JsonSerializer.Serialize(doctor);
            StringContent content = new StringContent(doctorJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "api/doctor", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"api/doctor/delete/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "api/doctor");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            List<Doctor> doctors = JsonSerializer.Deserialize<List<Doctor>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctors;
        }

        public async Task<Doctor> GetDoctorByUserIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"api/doctor/{id}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            Doctor doctor = JsonSerializer.Deserialize<Doctor>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctor;
        }

        public async Task<List<Doctor>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"api/doctor/doctor/{departmentId}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            List<Doctor> doctors = JsonSerializer.Deserialize<List<Doctor>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return doctors;
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            var departments = await GetAllDepartmentsAsync();
            return departments.FirstOrDefault(d => d.Id == id);
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "api/department");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Department>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}