using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ClassLibrary.Domain;
using ClassLibrary.IRepository;

namespace WinUI.Proxy
{
    public class UserProxy : IUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://localhost:5005/";

        public UserProxy(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task AddUserAsync(User user)
        {
            string userJson = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "api/user", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteUserAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"api/user/delete/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + "api/user");
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            List<User> users = JsonSerializer.Deserialize<List<User>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + $"api/user/{id}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            User user = JsonSerializer.Deserialize<User>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return user;

        }
    }
}
