using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;

namespace WinUI.Proxy
{
    internal class PatientProxy : IPatientRepository
    {
        private readonly HttpClient _http_client;
        private readonly string _base_api_url = "http://localhost:5005/";

        public PatientProxy(HttpClient _http_client)
        {
            this._http_client = _http_client;
        }

        public async Task<List<Patient>> getAllPatientsAsync()
        {
            HttpResponseMessage response = await this._http_client.GetAsync(this._base_api_url + "api/patient");
            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            List<PatientHttpModel> patients_http = JsonSerializer.Deserialize<List<PatientHttpModel>>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return patients_http.Select(mapToDomainModel).ToList();
        }

        public async Task<Patient> getPatientByUserIdAsync(int id)
        {
            HttpResponseMessage response = await this._http_client.GetAsync($"{this._base_api_url}api/patient/{id}");
            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            PatientHttpModel patient_http = JsonSerializer.Deserialize<PatientHttpModel>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (patient_http == null)
                throw new Exception($"No patient found with user id {id}");

            return mapToDomainModel(patient_http);
        }

        public async Task addPatientAsync(Patient patient)
        {
            PatientHttpModel patient_http = mapToHttpModel(patient);

            string patient_json = JsonSerializer.Serialize(patient_http);
            StringContent content = new StringContent(patient_json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await this._http_client.PostAsync(this._base_api_url + "api/patient", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task deletePatientAsync(int id)
        {
            HttpResponseMessage response = await this._http_client.DeleteAsync($"{this._base_api_url}api/patient/{id}");
            response.EnsureSuccessStatusCode();
        }

        private Patient mapToDomainModel(PatientHttpModel _http_model)
        {
            return new Patient
            {
                userId = _http_model.user_id,
                bloodType = _http_model.blood_type,
                emergencyContact = _http_model.emergency_contact,
                allergies = _http_model.allergies,
                weight = _http_model.weight,
                height = _http_model.height
            };
        }

        private PatientHttpModel mapToHttpModel(Patient _domain_model)
        {
            return new PatientHttpModel
            {
                user_id = _domain_model.userId,
                blood_type = _domain_model.bloodType,
                emergency_contact = _domain_model.emergencyContact,
                allergies = _domain_model.allergies,
                weight = _domain_model.weight,
                height = _domain_model.height
            };
        }

        private class PatientHttpModel
        {
            [JsonPropertyName("userId")]
            public int user_id { get; set; }

            [JsonPropertyName("patientName")]
            public string patient_name { get; set; }

            [JsonPropertyName("email")]
            public string email { get; set; }

            [JsonPropertyName("username")]
            public string username { get; set; }

            [JsonPropertyName("address")]
            public string address { get; set; }

            [JsonPropertyName("phoneNumber")]
            public string phone_number { get; set; }

            [JsonPropertyName("password")]
            public string password { get; set; }

            [JsonPropertyName("bloodType")]
            public string blood_type { get; set; }

            [JsonPropertyName("emergencyContact")]
            public string emergency_contact { get; set; }

            [JsonPropertyName("allergies")]
            public string allergies { get; set; }

            [JsonPropertyName("birthDate")]
            public DateOnly birth_date { get; set; }

            [JsonPropertyName("cnp")]
            public string cnp { get; set; }

            [JsonPropertyName("registrationDate")]
            public DateTime registration_date { get; set; }

            [JsonPropertyName("weight")]
            public double weight { get; set; }

            [JsonPropertyName("height")]
            public int height { get; set; }
        }
    }
}
