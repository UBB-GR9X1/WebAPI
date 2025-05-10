// <copyright file="ILoggerService.cs"  company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUI.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using WinUI.Model;

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
        /// <param name="_user_id">The ID of the user whose logs to retrieve.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getLogsByUserId(int _user_id);

        /// <summary>
        /// Retrieves logs of a specific action type.
        /// </summary>
        /// <param name="_action_type">The type of action to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getLogsByActionType(ActionType _action_type);

        /// <summary>
        /// Retrieves logs recorded before a specific timestamp.
        /// </summary>
        /// <param name="_timestamp">The cutoff timestamp.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getLogsBeforeTimestamp(DateTime _timestamp);

        /// <summary>
        /// Retrieves logs that match multiple filter criteria.
        /// </summary>
        /// <param name="_user_id">Optional user ID to filter by.</param>
        /// <param name="_action_type">The action type to filter by.</param>
        /// <param name="_timestamp">The timestamp to filter by.</param>
        /// <returns>A task representing the asynchronous operation with a list of log entries.</returns>
        Task<List<LogEntryModel>> getLogsWithParameters(int? _user_id, ActionType _action_type, DateTime _timestamp);

        /// <summary>
        /// Records a new action in the log.
        /// </summary>
        /// <param name="_user_id">The ID of the user who performed the action.</param>
        /// <param name="_action_type">The type of action performed.</param>
        /// <returns>A task representing the asynchronous operation with a boolean indicating success.</returns>
        Task<bool> logAction(int _user_id, ActionType _action_type);
    }
}
