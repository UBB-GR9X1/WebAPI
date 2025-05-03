using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ClassLibrary.IRepository;
using Domain;

namespace WinUI.Proxy
{
    internal class PatientProxy : IPatientRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5005/";

        public PatientProxy(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "api/patient");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            List<PatientHttpModel> patientsHttp = JsonSerializer.Deserialize<List<PatientHttpModel>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return patientsHttp.Select(MapToDomainModel).ToList();
        }

        public async Task<Patient> GetPatientByUserIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}api/patient/{id}");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            PatientHttpModel patientHttp = JsonSerializer.Deserialize<PatientHttpModel>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (patientHttp == null)
                throw new Exception($"No patient found with user id {id}");

            return MapToDomainModel(patientHttp);
        }

        public async Task AddPatientAsync(Patient patient)
        {
            PatientHttpModel patientHttp = MapToHttpModel(patient);

            string patientJson = JsonSerializer.Serialize(patientHttp);
            StringContent content = new StringContent(patientJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "api/patient", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePatientAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}api/patient/{id}");
            response.EnsureSuccessStatusCode();
        }

        private Patient MapToDomainModel(PatientHttpModel httpModel)
        {
            return new Patient
            {
                UserId = httpModel.UserId,
                BloodType = httpModel.BloodType,
                EmergencyContact = httpModel.EmergencyContact,
                Allergies = httpModel.Allergies,
                Weight = httpModel.Weight,
                Height = httpModel.Height
            };
        }

        private PatientHttpModel MapToHttpModel(Patient domainModel)
        {
            return new PatientHttpModel
            {
                UserId = domainModel.UserId,
                BloodType = domainModel.BloodType,
                EmergencyContact = domainModel.EmergencyContact,
                Allergies = domainModel.Allergies,
                Weight = domainModel.Weight,
                Height = domainModel.Height
            };
        }

        private class PatientHttpModel
        {
            [JsonPropertyName("userId")]
            public int UserId { get; set; }

            [JsonPropertyName("bloodType")]
            public string BloodType { get; set; }

            [JsonPropertyName("emergencyContact")]
            public string EmergencyContact { get; set; }

            [JsonPropertyName("allergies")]
            public string Allergies { get; set; }

            [JsonPropertyName("weight")]
            public double Weight { get; set; }

            [JsonPropertyName("height")]
            public int Height { get; set; }
        }
    }
}
