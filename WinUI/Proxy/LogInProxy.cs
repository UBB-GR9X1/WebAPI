using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ClassLibrary.Domain;
using WinUI.Exceptions;
using WinUI.Model;
using WinUI.Repository;

namespace WinUI.Proxy
{
    internal class LogInProxy : ILogInRepository
    {
        private readonly HttpClient _http_client;
        private readonly string s_base_url = Config._base_api_url;

        public LogInProxy(HttpClient http_client)
        {
            this._http_client = http_client;
        }

        public async Task<bool> authenticationLogService(int user_id, ActionType action_type_login_or_logout)
        {
            UserLogHttpModel log = new UserLogHttpModel
            {
                log_id = 0, // Assuming the server auto-generates this
                user_id = user_id,
                action_type = action_type_login_or_logout.convertToString(),
                timestamp = DateTime.UtcNow
            };

            string log_json = JsonSerializer.Serialize(log);
            StringContent content = new StringContent(log_json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _http_client.PostAsync(s_base_url + "log", content);
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> createAccount(UserCreateAccountModel model_for_creating_user_account)
        {
            HttpResponseMessage response = await this._http_client.GetAsync(this.s_base_url + "user");
            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            List<UserHttpModel> users = JsonSerializer.Deserialize<List<UserHttpModel>>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            bool exists = users.Any(u => u.username == model_for_creating_user_account.username
                                      || u.cnp == model_for_creating_user_account.cnp
                                      || u.mail == model_for_creating_user_account.mail);

            if (exists) throw new AuthenticationException("User already exists!");
            if (exists) return false;

            UserHttpModel _user_json = new UserHttpModel
            {
                user_id = 0,
                username = model_for_creating_user_account.username,
                password = model_for_creating_user_account.password,
                mail = model_for_creating_user_account.mail,
                role = "Patient",
                name = model_for_creating_user_account.name,
                birth_date = model_for_creating_user_account.birthDate,
                cnp = model_for_creating_user_account.cnp,
                address = "",
                phone_number = "0711111111",
                registration_date = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(_user_json);
            HttpContent _content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage _post_response = await this._http_client.PostAsync(this.s_base_url + "user", _content);
            _post_response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<UserAuthModel> getUserByUsername(string username)
        {
            HttpResponseMessage response = await this._http_client.GetAsync(this.s_base_url + "user");
            response.EnsureSuccessStatusCode();

            string response_body = await response.Content.ReadAsStringAsync();

            List<UserHttpModel> users = JsonSerializer.Deserialize<List<UserHttpModel>>(response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].username == username)
                {
                    return new UserAuthModel(users[i].user_id, users[i].username, users[i].password, users[i].mail, users[i].role);
                }
            }

            throw new AuthenticationException("No user found with given username");
        }

        public async Task<UserAuthModel> getUserById(int _user_id)
        {
            HttpResponseMessage _response = await this._httpClient.GetAsync(this._baseUrl + "api/user");
            _response.EnsureSuccessStatusCode();
            string _response_body = await _response.Content.ReadAsStringAsync();
            List<UserHttpModel> _users = JsonSerializer.Deserialize<List<UserHttpModel>>(_response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            for (int i = 0; i < _users.Count; i++)
            {
                if (_users[i].user_id == _user_id)
                {
                    return new UserAuthModel(_users[i].user_id, _users[i].username, _users[i].password, _users[i].mail, _users[i].role);
                }
            }
            throw new AuthenticationException("No user found with given id");
        }


        private class UserHttpModel
        public class UserHttpModel
        {
            [JsonPropertyName("userId")]
            public int user_id { get; set; }

            [JsonPropertyName("username")]
            public string username { get; set; }

            [JsonPropertyName("password")]
            public string password { get; set; }

            [JsonPropertyName("mail")]
            public string mail { get; set; }

            [JsonPropertyName("role")]
            public string role { get; set; }

            [JsonPropertyName("name")]
            public string name { get; set; }

            [JsonPropertyName("birthDate")]
            public DateOnly birth_date { get; set; }

            [JsonPropertyName("cnp")]
            public string cnp { get; set; }

            [JsonPropertyName("address")]
            public string address { get; set; }

            [JsonPropertyName("phoneNumber")]
            public string phone_number { get; set; }

            [JsonPropertyName("registrationDate")]
            public DateTime registration_date { get; set; }
        }

        private class UserLogHttpModel
        {
            [JsonPropertyName("logId")]
            public int log_id { get; set; }

            [JsonPropertyName("userId")]
            public int user_id { get; set; }

            [JsonPropertyName("actionType")]
            public string action_type { get; set; }

            [JsonPropertyName("timestamp")]
            public DateTime timestamp { get; set; } // ISO 8601 string format
        }
    }
}
