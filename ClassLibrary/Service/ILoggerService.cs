// <copyright file="ILoggerService.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace ClassLibrary.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ClassLibrary.Model;

    /// <summary>
    /// Interface for managing system logging operations.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Retrieves all logs from the system.
        /// </summary>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getAllLogs();

        /// <summary>
        /// Retrieves logs for a specific user.
        /// </summary>
        /// <param name="user_id">The ID of the user whose logs to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getLogsByUserId(int user_id);

        /// <summary>
        /// Retrieves logs of a specific action type.
        /// </summary>
        /// <param name="action_type">The type of action to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getLogsByActionType(ActionType action_type);

        /// <summary>
        /// Retrieves logs recorded before a specific timestamp.
        /// </summary>
        /// <param name="timestamp">The cutoff timestamp.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getLogsBeforeTimestamp(DateTime timestamp);

        /// <summary>
        /// Retrieves logs that match multiple filter criteria.
        /// </summary>
        /// <param name="user_id">Optional user ID to filter by.</param>
        /// <param name="action_type">The action type to filter by.</param>
        /// <param name="timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getLogsWithParameters(int? user_id, ActionType action_type, DateTime timestamp);

        /// <summary>
        /// Records a new action in the log.
        /// </summary>
        /// <param name="user_id">The ID of the user who performed the action.</param>
        /// <param name="action_type">The type of action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> logAction(int user_id, ActionType action_type);

        Task deleteLog(int id);
        Task<LogEntryModel> getLogById(int id);
    }
}
