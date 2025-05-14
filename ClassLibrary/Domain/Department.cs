namespace ClassLibrary.Domain
{
    /// <summary>
    /// Represents a department in the system.
    /// </summary>
    public class Department
    {
        /// <summary>
        /// Gets or sets the unique identifier for the department.
        /// </summary>
        public int departmentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the department.
        /// </summary>
        public string departmentName { get; set; }
    }
}
