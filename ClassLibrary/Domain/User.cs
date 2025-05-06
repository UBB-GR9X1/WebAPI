namespace ClassLibrary.Domain
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string mail { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the user.
        /// </summary>
        public DateOnly birthDate { get; set; }

        /// <summary>
        /// Gets or sets the CNP of the user.
        /// </summary>
        public string cnp { get; set; }

        /// <summary>
        /// Gets or sets the address of the user.
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the user.
        /// </summary>
        public string phoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the registration date of the user.
        /// </summary>
        public DateTime registrationDate { get; set; }
    }
}
