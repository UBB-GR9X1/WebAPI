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
        public int logId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user associated with the log entry.
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the action type fot the log entry.
        /// </summary>
        public string actionType { get; set; }

        /// <summary>
        /// Gets or sets the timestemp of the log entry.
        /// </summary>
        public DateTime timestamp { get; set; }
    }
}