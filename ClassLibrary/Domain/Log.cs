namespace ClassLibrary.Domain
{
    /// <summary>
    /// Logs class.
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Gets or sets the unique identifier for the log entry.
        /// </summary>
        public int log_id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user associated with the log entry.
        /// </summary>
        public int user_id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the action type fot the log entry.
        /// </summary>
        public string action_type{ get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the log entry.
        /// </summary>
        public DateTime timestamp { get; set; }
    }
}
