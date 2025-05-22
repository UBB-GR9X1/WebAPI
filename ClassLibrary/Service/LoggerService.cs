// <copyright file="LoggerService.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ClassLibrary.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ClassLibrary.Domain;
    using ClassLibrary.Repository;
    using ClassLibrary.Model;

    /// <summary>
    /// Model for handling system logging operations.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILogRepository _log_repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerService"/> class.
        /// </summary>
        /// <param name="log_repository">The logger service interface.</param>
        public LoggerService(ILogRepository log_repository)
        {
            this._log_repository = log_repository ?? throw new ArgumentNullException(nameof(log_repository));
        }

        /// <summary>
        /// Retrieves all log entries from the system.
        /// </summary>
        /// <returns>A list of all log entries.</returns>
        public async Task<List<LogEntryModel>> getAllLogs()
        {
            List<Log> logs = await this._log_repository.getAllLogsAsync();
            return convertLogsToLogEntryModels(logs);
        }

        /// <summary>
        /// Retrieves log entries for a specific user.
        /// </summary>
        /// <param name="user_id">The ID of the user.</param>
        /// <returns>A list of log entries for the specified user.</returns>
        /// <exception cref="ArgumentException">Thrown when the user_id is invalid.</exception>
        public async Task<List<LogEntryModel>> getLogsByUserId(int user_id)
        {
            if (user_id <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(user_id));
            }

            List<Log> logs = await this._log_repository.getAllLogsAsync();
            List<Log> filtered_logs = logs.FindAll(log => log.userId == user_id);
            return convertLogsToLogEntryModels(filtered_logs);
        }

        /// <summary>
        /// Retrieves log entries for a specific action type.
        /// </summary>
        /// <param name="action_type">The type of action to filter by.</param>
        /// <returns>A list of log entries for the specified action type.</returns>
        public async Task<List<LogEntryModel>> getLogsByActionType(ActionType action_type)
        {
            List<Log> logs = await this._log_repository.getAllLogsAsync();
            List<Log> filteredLogs = logs.FindAll(log => log.actionType == action_type.ToString());
            return convertLogsToLogEntryModels(filteredLogs);
        }

        /// <summary>
        /// Retrieves log entries before a specific timestamp.
        /// </summary>
        /// <param name="timestamp">The cutoff timestamp.</param>
        /// <returns>A list of log entries before the specified timestamp.</returns>
        /// <exception cref="ArgumentException">Thrown when the timestamp is default/uninitialized.</exception>
        public async Task<List<LogEntryModel>> getLogsBeforeTimestamp(DateTime timestamp)
        {
            if (timestamp == default)
            {
                throw new ArgumentException("Timestamp cannot be default.", nameof(timestamp));
            }

            List<Log> logs = await this._log_repository.getAllLogsAsync();
            List<Log> filteredLogs = logs.FindAll(log => log.timestamp < timestamp);
            return convertLogsToLogEntryModels(filteredLogs);
        }

        /// <summary>
        /// Retrieves log entries filtered by multiple parameters.
        /// </summary>
        /// <param name="user_id">The ID of the user, or null for all users.</param>
        /// <param name="action_type">The type of action.</param>
        /// <param name="timestamp">The cutoff timestamp.</param>
        /// <returns>A list of log entries matching the specified filters.</returns>
        public async Task<List<LogEntryModel>> getLogsWithParameters(int? user_id, ActionType action_type, DateTime timestamp)
        {
            List<Log> logs = await this._log_repository.getAllLogsAsync();

            if (user_id.HasValue)
            {
                List<Log> filteredLogs = logs.FindAll(log =>
                    log.userId == user_id.Value &&
                    log.actionType == action_type.ToString() &&
                    log.timestamp < timestamp);
                return convertLogsToLogEntryModels(filteredLogs);
            }
            else
            {
                List<Log> filteredLogs = logs.FindAll(log =>
                    log.actionType == action_type.ToString() &&
                    log.timestamp < timestamp);
                return convertLogsToLogEntryModels(filteredLogs);
            }
        }

        /// <summary>
        /// Logs a user action in the system.
        /// </summary>
        /// <param name="user_id">The ID of the user performing the action.</param>
        /// <param name="action_type">The type of action being performed.</param>
        /// <returns>True if the action was logged successfully, otherwise false.</returns>
        /// <exception cref="ArgumentException">Thrown when the user_id is invalid.</exception>
        public async Task<bool> logAction(int user_id, ActionType action_type)
        {
            if (user_id <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(user_id));
            }

            try
            {
                Log log = new Log
                {
                    userId = user_id,
                    actionType = action_type.ToString(),
                    timestamp = DateTime.UtcNow
                };

                await this._log_repository.addLogAsync(log);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<LogEntryModel> getLogById(int id)
        {
            var log = await _log_repository.getLogByIdAsync(id);
            if (log == null)
                return null;
            if (Enum.TryParse<ActionType>(log.actionType, out ActionType actionType))
            {
                return new LogEntryModel(
                    log.logId,
                    log.userId,
                    actionType,
                    log.timestamp
                );
            }

            return null;
        }

        public async Task deleteLog(int id)
        {
            await _log_repository.deleteLogAsync(id);
        }

        /// <summary>
        /// Converts Log domain objects to LogEntryModel objects.
        /// </summary>
        /// <param name="logs">The _logs to convert.</param>
        /// <returns>A list of LogEntryModel objects.</returns>
        private List<LogEntryModel> convertLogsToLogEntryModels(List<Log> logs)
        {
            if (logs == null)
                return new List<LogEntryModel>();

            List<LogEntryModel> result = new List<LogEntryModel>();

            foreach (Log log in logs)
            {
                if (Enum.TryParse<ActionType>(log.actionType, out ActionType actionType))
                {
                    result.Add(new LogEntryModel(
                        log.logId,
                        log.userId,
                        actionType,
                        log.timestamp
                    ));
                }
            }

            return result;
        }
    }
}
