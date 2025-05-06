using ClassLibrary.IRepository;
using ClassLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WinUI.Proxy
{
    /// <summary>
    /// Provides proxy methods to interact with the Notification Web API.
    /// </summary>
    public class NotificationProxy : INotificationRepository
    {
        /// <summary>
        /// The HTTP client used for sending requests to the Web API.
        /// </summary>

        private readonly HttpClient _http_client;
        public NotificationProxy(HttpClient _http_client)
        {
            this._http_client = _http_client;
        }

        /// <inheritdoc/>
        public async Task<List<Notification>> getAllNotificationsAsync()
        {
            var response = await _http_client.GetAsync("https://localhost:7004/api/notification");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var notifications = JsonSerializer.Deserialize<List<Notification>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return notifications ?? new List<Notification>();
        }

        /// <inheritdoc/>
        public async Task<List<Notification>> getNotificationsByUserIdAsync(int userId)
        {
            var response = await _http_client.GetAsync($"https://localhost:7004/api/notification/user/{userId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var notifications = JsonSerializer.Deserialize<List<Notification>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return notifications ?? new List<Notification>();
        }

        /// <inheritdoc/>
        public async Task<Notification> getNotificationByIdAsync(int id)
        {
            var response = await _http_client.GetAsync($"https://localhost:7004/api/notification/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Notification with ID {id} was not found.");
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var notification = JsonSerializer.Deserialize<Notification>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return notification ?? throw new Exception("Failed to deserialize notification.");
        }

        /// <inheritdoc/>
        public async Task addNotificationAsync(Notification notification)
        {
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json");

            var response = await _http_client.PostAsync("https://localhost:7004/api/notification", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc/>
        public async Task deleteNotificationAsync(int id)
        {
            var response = await _http_client.DeleteAsync($"https://localhost:7004/api/notification/delete/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Notification with ID {id} was not found.");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
