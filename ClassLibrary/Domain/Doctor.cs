namespace ClassLibrary.Domain
{
    /// <summary>
    /// Represents a doctor in the system.
    /// </summary>
    public class Doctor
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// Gets or sets the departement identifier for the doctor.
        /// </summary>
        public int departmentId { get; set; }

        /// <summary>
        /// Gets or sets doctor rating for the doctor.
        /// </summary>
        public double doctorRating { get; set; }

        /// <summary>
        /// Gets or sets the License number for the doctor.
        /// </summary>
        public string licenseNumber { get; set; }
    }
}