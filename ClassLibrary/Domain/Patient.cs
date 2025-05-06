namespace ClassLibrary.Domain
{
    /// <summary>
    /// Represents a patient in the system.
    /// </summary>
    public class Patient
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// Gets or sets the blood type of the patient.
        /// </summary>
        public string bloodType { get; set; }

        /// <summary>
        /// Gets or sets the emergency contact information of the patient.
        /// </summary>
        public string emergencyContact { get; set; }

        /// <summary>
        /// Gets or sets the allergies of the patient.
        /// </summary>
        public string allergies { get; set; }

        /// <summary>
        /// Gets or sets the weight of the patient
        /// </summary>
        public double weight { get; set; }

        /// <summary>
        /// Gets or sets the height of the patient
        /// </summary>
        public int height { get; set; }
    }
}