// <copyright file="LoggerProxy.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>


namespace ClassLibrary.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using ClassLibrary.Domain;
    using ClassLibrary.Repository;
    using ClassLibrary.Model;

    /// <summary>
    /// Service for handling database operations related to system logs.
    /// </summary>
    public class LoggerProxy : ILogRepository
    {
        private readonly HttpClient _http_client;
        private static readonly string s_base_api_url = "http://localhost:5005/api/";
        private readonly JsonSerializerOptions _json_options;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerProxy"/> class.
        /// </summary>
        public LoggerProxy()
        {
            this._http_client = new HttpClient();

            this._json_options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        /// <summary>
        /// Gets all logs.
        /// </summary>
        /// <returns>A list of all logs.</returns>
        public async Task<List<Log>> getAllLogsAsync()
        {
            try
            {
                HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + "log");
                response.EnsureSuccessStatusCode();

                List<ApiLogDto> apiLogs = await response.Content.ReadFromJsonAsync<List<ApiLogDto>>(this._json_options);
                return convertApiLogsToLogs(apiLogs);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                return new List<Log>();
            }
        }

        /// <summary>
        /// Gets a log by its unique identifier.
        /// </summary>
        /// <param name="id">The id of the log.</param>
        /// <returns>The log with the given id.</returns>
        public async Task<Log> getLogByIdAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + $"log/{id}");
                response.EnsureSuccessStatusCode();

                ApiLogDto apiLog = await response.Content.ReadFromJsonAsync<ApiLogDto>(this._json_options);
                return convertApiLogToLog(apiLog);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                throw new Exception($"Log with ID {id} not found.");
            }
        }

        /// <summary>
        /// Gets a log by its user id.
        /// </summary>
        /// <param name="user_id">The id of the user.</param>
        /// <returns>The log with the given user id.</returns>
        public async Task<Log> getLogByUserIdAsync(int user_id)
        {
            try
            {
                HttpResponseMessage response = await this._http_client.GetAsync(s_base_api_url + "log");
                response.EnsureSuccessStatusCode();

                List<ApiLogDto> apiLogs = await response.Content.ReadFromJsonAsync<List<ApiLogDto>>(this._json_options);
                var userLog = apiLogs.Find(log => log.userId == user_id);

                if (userLog == null)
                {
                    throw new Exception($"Log for user ID {user_id} not found.");
                }

                return convertApiLogToLog(userLog);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                throw;
            }
        }

        /// <summary>
        /// Adds a new log to the system.
        /// </summary>
        /// <param name="log">The log to be added.</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task addLogAsync(Log log)
        {
            try
            {
                ApiLogDto log_data = new ApiLogDto
                {
                    userId = log.userId,
                    actionType = log.actionType,
                    timestamp = log.timestamp
                };

                HttpResponseMessage response = await this._http_client.PostAsJsonAsync(s_base_api_url + "log", log_data);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a log by its unique identifier.
        /// </summary>
        /// <param name="id">The id of the log</param>
        /// <returns>Task representing the asynchronous operation.</returns>
        public async Task deleteLogAsync(int id)
        {
            try
            {
                HttpResponseMessage response = await this._http_client.DeleteAsync(s_base_api_url + $"log/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                throw;
            }
        }

        /// <summary>
        /// Converts API log DTOs to Log domain objects.
        /// </summary>
        /// <param name="_api_logs">The API log DTOs to convert.</param>
        /// <returns>A list of Log domain objects.</returns>
        private List<Log> convertApiLogsToLogs(List<ApiLogDto> _api_logs)
        {
            if (_api_logs == null)
                return new List<Log>();

            List<Log> result = new List<Log>();

            foreach (ApiLogDto api_log in _api_logs)
            {
                result.Add(new Log
                {
                    logId = api_log.logId,
                    userId = api_log.userId,
                    actionType = api_log.actionType,
                    timestamp = api_log.timestamp
                });
            }

            return result;
        }

        /// <summary>
        /// Converts a single API log DTO to a Log domain object.
        /// </summary>
        /// <param name="_api_log">The API log DTO to convert.</param>
        /// <returns>A Log domain object.</returns>
        private Log convertApiLogToLog(ApiLogDto _api_log)
        {
            if (_api_log == null)
                return null;

            return new Log
            {
                logId = _api_log.logId,
                userId = _api_log.userId,
                actionType = _api_log.actionType,
                timestamp = _api_log.timestamp
            };
        }
    }
}

