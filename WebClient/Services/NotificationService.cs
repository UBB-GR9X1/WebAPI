using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebClient.Models;

namespace WebClient.Services
{
    public class NotificationService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "api/notification";

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(int userId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Notification>>($"{BaseUrl}/user/{userId}");
            }
            catch (Exception)
            {
                return new List<Notification>();
            }
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseUrl}/delete/{notificationId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}