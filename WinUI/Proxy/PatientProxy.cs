using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WinUI.Repository;
using ClassLibrary.Domain;
using WinUI.Model;
using static WinUI.Proxy.LogInProxy;
using ClassLibrary.Repository;

namespace WinUI.Proxy
{
    internal class PatientProxy : IPatientRepository
    {
        private readonly HttpClient _http_client;
        private readonly string s_base_api_url = Config._base_api_url;

        public PatientProxy(HttpClient http_client)
        {
            this._http_client = http_client;
        }

        public async Task<List<User>> getAllUserAsync()
        {
            HttpResponseMessage response = await this._http_client.GetAsync(this.s_base_api_url + "user");
            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            List<UserHttpModel> users = JsonSerializer.Deserialize<List<UserHttpModel>>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            //filter Patients
            List<UserHttpModel> filtered_users = users.FindAll(user => user.role == "Patient");
            return convertToUser(filtered_users);
        }

        private List<User> convertToUser(List<UserHttpModel> http_users)
        {
            List<User> users = new List<User>();

            foreach (var httpUser in http_users)
            {
                users.Add(new User
                {
                    userId = httpUser.user_id,
                    username = httpUser.username,
                    password = httpUser.password,
                    mail = httpUser.mail,
                    role = httpUser.role,
                    name = httpUser.name,
                    birthDate = httpUser.birth_date,
                    cnp = httpUser.cnp,
                    address = httpUser.address,
                    phoneNumber = httpUser.phone_number,
                    registrationDate = httpUser.registration_date
                });
            }

            return users;
        }

        public async Task<List<Patient>> getAllPatientsAsync()
        {
            HttpResponseMessage response = await this._http_client.GetAsync(this.s_base_api_url + "patient");
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
            HttpResponseMessage response = await this._http_client.GetAsync($"{this.s_base_api_url}patient/{id}");
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

            HttpResponseMessage response = await this._http_client.PostAsync(this.s_base_api_url + "patient", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task updatePatientAsync(Patient patient, User user)
        {
            try
            {
                PatientHttpModel patient_http = mapToHttpModel(patient);
                string patient_json = JsonSerializer.Serialize(patient_http);
                StringContent patient_content = new StringContent(patient_json, Encoding.UTF8, "application/json");

                HttpResponseMessage patient_response = await this._http_client.PostAsync($"{this.s_base_api_url}patient", patient_content);
                patient_response.EnsureSuccessStatusCode();

                UserHttpModel user_http = mapUserToHttpModel(user);
                string user_json = JsonSerializer.Serialize(user_http);
                StringContent user_content = new StringContent(user_json, Encoding.UTF8, "application/json");

                // Use PUT for updating existing user data
                HttpResponseMessage user_response = await this._http_client.PutAsync($"{this.s_base_api_url}user/{user.userId}", user_content);
                user_response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating patient data: {ex.Message}", ex);
            }
        }

        private UserHttpModel mapUserToHttpModel(User user)
        {
            return new UserHttpModel
            {
                user_id = user.userId,
                username = user.username,
                password = user.password,
                mail = user.mail,
                role = user.role,
                name = user.name,
                birth_date = user.birthDate,
                cnp = user.cnp,
                address = user.address,
                phone_number = user.phoneNumber,
                registration_date = user.registrationDate
            };
        }

        public async Task deletePatientAsync(int id)
        {
            HttpResponseMessage response = await this._http_client.DeleteAsync($"{this.s_base_api_url}patient/{id}");
            response.EnsureSuccessStatusCode();
        }

        private Patient mapToDomainModel(PatientHttpModel _http_model)
        {
            return new Patient
            {
                userId = _http_model.userId,
                bloodType = _http_model.bloodType,
                EmergencyContact = _http_model.emergencyContact,
                allergies = _http_model.allergies,
                weight = _http_model.weight,
                height = _http_model.height
            };
        }

        private PatientHttpModel mapToHttpModel(Patient _domain_model)
        {
            return new PatientHttpModel
            {
                userId = _domain_model.userId,
                bloodType = _domain_model.bloodType,
                emergencyContact = _domain_model.EmergencyContact,
                allergies = _domain_model.allergies,
                weight = _domain_model.weight,
                height = _domain_model.height
            };
        }

        private class PatientHttpModel
        {
            [JsonPropertyName("userId")]
            public int userId { get; set; }

            [JsonPropertyName("patientName")]
            public string patientName { get; set; }

            [JsonPropertyName("email")]
            public string email { get; set; }

            [JsonPropertyName("username")]
            public string username { get; set; }

            [JsonPropertyName("address")]
            public string address { get; set; }

            [JsonPropertyName("phoneNumber")]
            public string phoneNumber { get; set; }

            [JsonPropertyName("password")]
            public string password { get; set; }

            [JsonPropertyName("bloodType")]
            public string bloodType { get; set; }

            [JsonPropertyName("emergencyContact")]
            public string emergencyContact { get; set; }

            [JsonPropertyName("allergies")]
            public string allergies { get; set; }

            [JsonPropertyName("birthDate")]
            public DateOnly birthDate { get; set; }

            [JsonPropertyName("cnp")]
            public string cnp { get; set; }

            [JsonPropertyName("registrationDate")]
            public DateTime registrationDate { get; set; }

            [JsonPropertyName("weight")]
            public double weight { get; set; }

            [JsonPropertyName("height")]
            public int height { get; set; }
        }
    }
}
