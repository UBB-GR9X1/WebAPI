// <copyright file="LoggerProxy.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Repository;

namespace WinUI.Proxy
{
    /// <summary>
    /// Service for handling database operations related to system logs.
    /// </summary>
    public class LoggerProxy : ILoggerRepository
    {
        private readonly HttpClient _http_client;
        private static readonly string s_base_api_url = Config._base_api_url;
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
        /// Gets all logs from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> getAllLogs()
        {
            try
            {
                // The ApiLogDto is a temporary class to handle the API response
                HttpResponseMessage _response = await this._http_client.GetAsync(s_base_api_url + "api/log");
                _response.EnsureSuccessStatusCode();

                List<ApiLogDto> _api_logs = await _response.Content.ReadFromJsonAsync<List<ApiLogDto>>(this._json_options);
                return this.convertApiLogsToLogEntryModels(_api_logs);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                return new List<LogEntryModel>();
            }
        }

        /// <summary>
        /// Gets logs for a specific user.
        /// </summary>
        /// <param name="_user_id">The ID of the user whose logs to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> getLogsByUserId(int _user_id)
        {
            try
            {
                HttpResponseMessage _response = await this._http_client.GetAsync(s_base_api_url + "api/log");
                _response.EnsureSuccessStatusCode();
                List<ApiLogDto> _api_logs = await _response.Content.ReadFromJsonAsync<List<ApiLogDto>>(this._json_options);
                List<ApiLogDto> _filtered_api_logs = _api_logs.FindAll(log => log._user_id == _user_id);
                return this.convertApiLogsToLogEntryModels(_filtered_api_logs);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                return new List<LogEntryModel>();
            }
        }
        

        /// <summary>
        /// Gets logs from before a specific timestamp.
        /// </summary>
        /// <param name="_before_timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> getLogsBeforeTimestamp(DateTime _before_timestamp)
        {
            try
            {
                HttpResponseMessage _response = await this._http_client.GetAsync(s_base_api_url + "api/log");
                _response.EnsureSuccessStatusCode();
                List<ApiLogDto> _api_logs = await _response.Content.ReadFromJsonAsync<List<ApiLogDto>>(this._json_options);
                List<ApiLogDto> _filtered_api_logs = _api_logs.FindAll(_log => _log._timestamp < _before_timestamp);
                return this.convertApiLogsToLogEntryModels(_filtered_api_logs);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                return new List<LogEntryModel>();
            }
        }

        /// <summary>
        /// Records a new action in the log.
        /// </summary>
        /// <param name="_user_id">The ID of the user who performed the action.</param>
        /// <param name="_action_type">The type of action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        public async Task<bool> logAction(int _user_id, ActionType _action_type)
        {
            try
            {
                ApiLogDto _log_data = new ApiLogDto
                {
                    _user_id = _user_id,
                    _action_type = _action_type.ToString(),
                    _timestamp = DateTime.UtcNow
                };

                HttpResponseMessage _response = await this._http_client.PostAsJsonAsync(s_base_api_url + "api/log", _log_data);
                return _response.IsSuccessStatusCode;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets logs by action type.
        /// </summary>
        /// <param name="_action_type">The action type to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> getLogsByActionType(ActionType _action_type)
        {
            try
            {
                HttpResponseMessage _response = await this._http_client.GetAsync(s_base_api_url + "api/log");
                _response.EnsureSuccessStatusCode();
                List<ApiLogDto> _api_logs = await _response.Content.ReadFromJsonAsync<List<ApiLogDto>>(this._json_options);
                List<ApiLogDto> _filtered_api_logs = _api_logs.FindAll(_log => _log._action_type == _action_type.ToString());
                return this.convertApiLogsToLogEntryModels(_filtered_api_logs);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                return new List<LogEntryModel>();
            }
        }

        /// <summary>
        /// Gets logs matching specific parameters without filtering by user ID.
        /// </summary>
        /// <param name="_action_type">The action type to filter by.</param>
        /// <param name="_before_timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> getLogsWithParametersWithoutUserId(ActionType _action_type, DateTime _before_timestamp)
        {
            try
            {
                HttpResponseMessage _response = await this._http_client.GetAsync(s_base_api_url + "api/log");
                _response.EnsureSuccessStatusCode();
                List<ApiLogDto> _api_logs = await _response.Content.ReadFromJsonAsync<List<ApiLogDto>>(this._json_options);
                List<ApiLogDto> _filtered_api_logs = _api_logs.FindAll(_log => 
                    _log._action_type == _action_type.ToString() &&
                    _log._timestamp < _before_timestamp);
                return this.convertApiLogsToLogEntryModels(_filtered_api_logs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<LogEntryModel>();
            }
        }

        /// <summary>
        /// Gets logs matching specific parameters.
        /// </summary>
        /// <param name="_user_id">The user ID to filter by.</param>
        /// <param name="_action_type">The action type to filter by.</param>
        /// <param name="_before_timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        public async Task<List<LogEntryModel>> getLogsWithParameters(int _user_id, ActionType _action_type, DateTime _before_timestamp)
        {
            try
            {
                HttpResponseMessage _response = await this._http_client.GetAsync(s_base_api_url + "api/log");
                _response.EnsureSuccessStatusCode();
                List<ApiLogDto> _api_logs = await _response.Content.ReadFromJsonAsync<List<ApiLogDto>>(this._json_options);
                List<ApiLogDto> _filtered_api_logs = _api_logs.FindAll(_log =>
                    _log._user_id == _user_id &&
                    _log._action_type == _action_type.ToString() &&
                    _log._timestamp < _before_timestamp);
                return this.convertApiLogsToLogEntryModels(_filtered_api_logs);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                return new List<LogEntryModel>();
            }
        }

        /// <summary>
        /// Converts API log models to internal log entry models.
        /// </summary>
        private List<LogEntryModel> convertApiLogsToLogEntryModels(List<ApiLogDto> _api_logs)
        {
            if (_api_logs == null)
                return new List<LogEntryModel>();

            List<LogEntryModel> result = new List<LogEntryModel>();

            foreach (ApiLogDto _api_log in _api_logs)
            {
                if (Enum.TryParse<ActionType>(_api_log._action_type, out ActionType _action_type))
                {
                    result.Add(new LogEntryModel(
                        _api_log._log_id,
                        _api_log._user_id,
                        _action_type,
                        _api_log._timestamp
                    ));
                }
            }

            return result;
        }
    }
}

