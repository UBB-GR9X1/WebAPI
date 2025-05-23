using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ClassLibrary.Domain;
using ClassLibrary.Repository;

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

        public async Task addUserAsync(User user)
        {
            string userJson = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_baseUrl + "api/user", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task deleteUserAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(_baseUrl + $"api/user/delete/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<User>> getAllUsersAsync()
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

        public Task<User> getUserByCNPAsync(string cnp)
        {
            throw new NotImplementedException();
        }

        public async Task<User> getUserByIdAsync(int id)
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

        public Task<List<User>> getUsersByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> getUsersByRoleAsync(string role)
        {
            throw new NotImplementedException();
        }

        public async Task updateUserAsync(User user)
        {
            string userJson = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_baseUrl + $"api/user/{user.userId}", content);
            response.EnsureSuccessStatusCode();
        }

    }
}
