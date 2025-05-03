using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WinUI.Exceptions;
using WinUI.Model;
using WinUI.Repository;

namespace WinUI.Proxy
{
    internal class LogInProxy : ILogInRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5005/";

        public LogInProxy(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<bool> authenticationLogService(int _user_id, ActionType _action_type_login_or_logout)
        {
            UserLogHttpModel log = new UserLogHttpModel
            {
                log_id = 0, // Assuming the server auto-generates this
                user_id = _user_id,
                action_type = _action_type_login_or_logout.convertToString(),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };

            string logJson = JsonSerializer.Serialize(log);
            StringContent content = new StringContent(logJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "api/log", content);
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<bool> createAccount(UserCreateAccountModel _model_for_creating_user_account)
        {
            HttpResponseMessage _response = await this._httpClient.GetAsync(this._baseUrl + "api/user");
            _response.EnsureSuccessStatusCode();

            string _response_body = await _response.Content.ReadAsStringAsync();

            List<UserHttpModel> _users = JsonSerializer.Deserialize<List<UserHttpModel>>(_response_body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            bool exists = _users.Any(u => u.username == _model_for_creating_user_account.username
                                      || u.cnp == _model_for_creating_user_account.cnp
                                      || u.mail == _model_for_creating_user_account.mail);

            if (exists) throw new AuthenticationException("User already exists!");
            if (exists) return false;

            string _user_json = JsonSerializer.Serialize(new UserHttpModel
            {
                user_id = 0,
                username = _model_for_creating_user_account.username,
                password = _model_for_creating_user_account.password,
                mail = _model_for_creating_user_account.mail,
                role = "USER",
                name = _model_for_creating_user_account.name,
                birth_date = _model_for_creating_user_account.birth_date.ToDateTime(new TimeOnly(0, 0)),
                cnp = _model_for_creating_user_account.cnp,
                address = "",
                phone_number = "",
                registration_date = DateTime.UtcNow
            });
            StringContent _content = new StringContent(_user_json, Encoding.UTF8, "application/json");
            HttpResponseMessage _post_response = await this._httpClient.PostAsync(this._baseUrl + "api/user", _content);
            _post_response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<UserAuthModel> getUserByUsername(string _username)
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
                if (_users[i].username == _username)
                {
                    return new UserAuthModel(_users[i].user_id, _users[i].username, _users[i].password, _users[i].mail, _users[i].role);
                }
            }

            throw new AuthenticationException("No user found with given username");
        }

        private class UserHttpModel
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
            public DateTime birth_date { get; set; }

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
            public string timestamp { get; set; } // ISO 8601 string format
        }
    }
}
