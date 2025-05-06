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
        private readonly HttpClient _http_client;
        private readonly string _base_url = "http://localhost:5005/";

        /// <summary>
        /// Initializes a new instance of the <see cref="LogInProxy"/> class with the specified HTTP client.
        /// </summary>
        /// <param name="http_client">The HTTP client instance to use for API communication.</param>
        public LogInProxy(HttpClient http_client)
        {
            this._http_client = http_client;
        }

        /// <summary>
        /// Sends a log entry for user authentication actions (login or logout) to the API.
        /// </summary>
        /// <param name="_user_id">The ID of the user performing the action.</param>
        /// <param name="_action_type_login_or_logout">The type of action (login or logout).</param>
        /// <returns>True if the log was successfully sent; otherwise, throws an exception.</returns>
        public async Task<bool> authenticationLogService(int _user_id, ActionType _action_type_login_or_logout)
        {
            UserLogHttpModel log = new UserLogHttpModel
            {
                log_id = 0, // Assuming the server auto-generates this
                user_id = _user_id,
                action_type = _action_type_login_or_logout.convertToString(),
                timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
            };

            string log_json = JsonSerializer.Serialize(log);
            StringContent content = new StringContent(log_json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _http_client.PostAsync(_base_url + "api/log", content);
            response.EnsureSuccessStatusCode();

            return true;
        }

        /// <summary>
        /// Creates a new user account by sending user details to the API. Checks for duplicates before creation.
        /// </summary>
        /// <param name="_model_for_creating_user_account">The model containing user account details.</param>
        /// <returns>True if the account was successfully created; otherwise, false if the user already exists.</returns>
        /// <exception cref="AuthenticationException">Thrown if a user with the same username, CNP, or email already exists.</exception>
        public async Task<bool> createAccount(UserCreateAccountModel _model_for_creating_user_account)
        {
            HttpResponseMessage _response = await this._http_client.GetAsync(this._base_url + "api/user");
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

            UserHttpModel _user_json = new UserHttpModel
            {
                user_id = 0,
                username = _model_for_creating_user_account.username,
                password = _model_for_creating_user_account.password,
                mail = _model_for_creating_user_account.mail,
                role = "Patient",
                name = _model_for_creating_user_account.name,
                birth_date = _model_for_creating_user_account.birth_date,
                cnp = _model_for_creating_user_account.cnp,
                address = "",
                phone_number = "0711111111",
                registration_date = DateTime.UtcNow
            };

            var _json = JsonSerializer.Serialize(_user_json);
            HttpContent _content = new StringContent(_json, Encoding.UTF8, "application/json");
            HttpResponseMessage _post_response = await this._httpClient.PostAsync(this._baseUrl + "api/user", _content);
            _post_response.EnsureSuccessStatusCode();

            return true;
        }

        /// <summary>
        /// Retrieves a user by username from the API.
        /// </summary>
        /// <param name="_username">The username to search for.</param>
        /// <returns>A <see cref="UserAuthModel"/> representing the user if found.</returns>
        /// <exception cref="AuthenticationException">Thrown if no user is found with the given username.</exception>
        public async Task<UserAuthModel> getUserByUsername(string _username)
        {
            HttpResponseMessage _response = await this._http_client.GetAsync(this._base_url + "api/user");
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
            public string timestamp { get; set; } // ISO 8601 string format
        }
    }
}
