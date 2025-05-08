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
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the departement identifier for the doctor.
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets doctor rating for the doctor.
        /// </summary>
        public double DoctorRating { get; set; }

        /// <summary>
        /// Gets or sets the License number for the doctor.
        /// </summary>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the doctor.
        /// </summary>
        public string Name { get; set; }
    }
}
