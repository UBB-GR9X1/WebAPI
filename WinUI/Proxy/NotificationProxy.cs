using ClassLibrary.Repository;
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
        private static readonly string s_base_api_url = Config._base_api_url;
        
        public NotificationProxy(HttpClient _http_client)
        {
            this._http_client = _http_client;
        }

        /// <inheritdoc/>
        public async Task<List<Notification>> getAllNotificationsAsync()
        {
            HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + "notification");
            response.EnsureSuccessStatusCode();

            String json = await response.Content.ReadAsStringAsync();
            List<Notification> notifications = JsonSerializer.Deserialize<List<Notification>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return notifications ?? new List<Notification>();
        }

        /// <inheritdoc/>
        public async Task<List<Notification>> getNotificationsByUserIdAsync(int user_id)
        {
            HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + $"notification/user/{user_id}");
            response.EnsureSuccessStatusCode();

            String json = await response.Content.ReadAsStringAsync();
            List<Notification> notifications = JsonSerializer.Deserialize<List<Notification>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return notifications ?? new List<Notification>();
        }

        /// <inheritdoc/>
        public async Task<Notification> getNotificationByIdAsync(int id)
        {
            HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url +  $"notification/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Notification with ID {id} was not found.");
            }

            response.EnsureSuccessStatusCode();

            string _json = await response.Content.ReadAsStringAsync();

            Notification _notification = JsonSerializer.Deserialize<Notification>(_json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return _notification ?? throw new Exception("Failed to deserialize notification.");
        }

        /// <inheritdoc/>
        public async Task addNotificationAsync(Notification notification)
        {
            StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(notification),
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response = await this._http_client.PostAsync(s_base_api_url +  "notification", jsonContent);
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc/>
        public async Task deleteNotificationAsync(int id)
        {
            HttpResponseMessage response = await this._http_client.DeleteAsync(s_base_api_url + $"notification/delete/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new KeyNotFoundException($"Notification with ID {id} was not found.");
            }

            response.EnsureSuccessStatusCode();
        }
    }
}
