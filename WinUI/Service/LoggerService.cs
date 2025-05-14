// <copyright file="LoggerService.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUI.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WinUI.Model;
    using WinUI.Repository;

    /// <summary>
    /// Model for handling system logging operations.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILoggerRepository _logger_repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerService"/> class.
        /// </summary>
        /// <param name="logger_repository">The logger service interface.</param>
        public LoggerService(ILoggerRepository logger_repository)
        {
            this._logger_repository = logger_repository ?? throw new ArgumentNullException(nameof(logger_repository));
        }

        /// <summary>
        /// Retrieves all log entries from the system.
        /// </summary>
        /// <returns>A list of all log entries.</returns>
        public async Task<List<LogEntryModel>> getAllLogs()
        {
            return await this._logger_repository.getAllLogs();
        }

        /// <summary>
        /// Retrieves log entries for a specific user.
        /// </summary>
        /// <param name="user_id">The ID of the user.</param>
        /// <returns>A list of log entries for the specified user.</returns>
        /// <exception cref="ArgumentException">Thrown when the _user_id is invalid.</exception>
        public async Task<List<LogEntryModel>> getLogsByUserId(int user_id)
        {
            if (user_id <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(user_id));
            }

            return await this._logger_repository.getLogsByUserId(user_id);
        }

        /// <summary>
        /// Retrieves log entries for a specific action type.
        /// </summary>
        /// <param name="action_type">The type of action to filter by.</param>
        /// <returns>A list of log entries for the specified action type.</returns>
        /// <exception cref="ArgumentNullException">This exception won't occur since _action_type is an enum, but kept for interface compliance.</exception>
        public async Task<List<LogEntryModel>> getLogsByActionType(ActionType action_type)
        {
            // _action_type is an enum, so it can't be null, but we're keeping this check
            // for interface compliance and future-proofing
            // The result of the expression is always 'false'
            if (action_type == null)
            {
                throw new ArgumentNullException(nameof(action_type), "Action type cannot be null.");
            }

            return await this._logger_repository.getLogsByActionType(action_type);
        }

        /// <summary>
        /// Retrieves log entries before a specific _timestamp.
        /// </summary>
        /// <param name="timestamp">The cutoff _timestamp.</param>
        /// <returns>A list of log entries before the specified _timestamp.</returns>
        /// <exception cref="ArgumentException">Thrown when the _timestamp is default/uninitialized.</exception>
        public async Task<List<LogEntryModel>> getLogsBeforeTimestamp(DateTime timestamp)
        {
            if (timestamp == default)
            {
                throw new ArgumentException("_timestamp cannot be default.", nameof(timestamp));
            }

            return await this._logger_repository.getLogsBeforeTimestamp(timestamp);
        }

        /// <summary>
        /// Retrieves log entries filtered by multiple parameters.
        /// </summary>
        /// <param name="user_id">The ID of the user, or null for all users.</param>
        /// <param name="action_type">The type of action.</param>
        /// <param name="timestamp">The cutoff _timestamp.</param>
        /// <returns>A list of log entries matching the specified filters.</returns>
        public async Task<List<LogEntryModel>> getLogsWithParameters(int? user_id, ActionType action_type, DateTime timestamp)
        {
            if (user_id != null)
            {
                return await this._logger_repository.getLogsWithParameters(user_id.Value, action_type, timestamp);
            }
            else
            {
                return await this._logger_repository.getLogsWithParametersWithoutUserId(action_type, timestamp);
            }
        }

        /// <summary>
        /// Logs a user action in the system.
        /// </summary>
        /// <param name="user_id">The ID of the user performing the action.</param>
        /// <param name="action_type">The type of action being performed.</param>
        /// <returns>True if the action was logged successfully, otherwise false.</returns>
        /// <exception cref="ArgumentException">Thrown when the _user_id is invalid.</exception>
        /// <exception cref="ArgumentNullException">This exception won't occur since action_type is an enum, but kept for interface compliance.</exception>
        public async Task<bool> logAction(int user_id, ActionType action_type)
        {
            if (user_id <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(user_id));
            }

            // action_type is an enum, so it can't be null, but we're keeping this check
            // for interface compliance and future-proofing
            if (action_type == null)
            {
                throw new ArgumentNullException(nameof(action_type), "Action type cannot be null.");
            }

            return await this._logger_repository.logAction(user_id, action_type);
        }
    }
}


