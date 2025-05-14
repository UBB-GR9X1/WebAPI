using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    public enum ActionType
    {
        /// <summary>
        /// User login action.
        /// </summary>
        LOGIN,

        /// <summary>
        /// User logout action.
        /// </summary>
        LOGOUT,

        /// <summary>
        /// Profile update action.
        /// </summary>
        UPDATE_PROFILE,

        /// <summary>
        /// password change action.
        /// </summary>
        CHANGE_PASSWORD,

        /// <summary>
        /// Account deletion action.
        /// </summary>
        DELETE_ACCOUNT,

        /// <summary>
        /// Account creation action.
        /// </summary>
        CREATE_ACCOUNT,
    }

    static class ActionTypeMethods
    {
        public static String convertToString(this ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.LOGIN:
                    return "LOGIN";
                case ActionType.LOGOUT:
                    return "LOGOUT";
                case ActionType.CREATE_ACCOUNT:
                    return "CREATE_ACCOUNT";
                default:
                    return "UNDEFINED";
            }
        }
    }

    /// <summary>
    /// Represents a log entry in the system's activity log.
    /// <param name="log_id">The unique identifier for the log entry.</param>
    /// <param name="user_id">The user ID associated with the action.</param>
    /// <param name="action">The type of action performed.</param>
    /// <param name="timestamp">The date and time when the action occurred.</param>
    public class LogEntryModel(int log_id, int user_id, ActionType action, DateTime timestamp)
    {
        /// <summary>
        /// Gets or sets the unique identifier for the log entry.
        /// </summary>
        public int log_id { get; set; } = log_id;

        /// <summary>
        /// Gets or sets the ID of the user who performed the action.
        /// </summary>
        public int user_id { get; set; } = user_id;
      
        /// <summary>
        /// Gets or sets the type of action performed.
        /// </summary>
        public ActionType action_type { get; set; } = action;

        /// <summary>
        /// Gets or sets the date and time when the action was performed.
        /// </summary>
        public DateTime timestamp { get; set; } = timestamp;

        /// <summary>
        /// Returns a string representation of the log entry.
        /// </summary>
        /// <returns>A string containing the log entry details.</returns>
        public override string ToString()
        {
            return $"log_id: {this.log_id}, user_id: {this.user_id}, Action: {this.action_type}, timestamp: {this.timestamp}";
        }
    }
}