using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Model
{

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreateAccountModel"/> class.
    /// </summary>
    /// <param name="username">username.</param>
    /// <param name="password">user password.</param>
    /// <param name="mail">user mail.</param>
    /// <param name="name">user's name.</param>
    /// <param name="birth_date">user's birthdate.</param>
    /// <param name="cnp">user's cnp.</param>
    /// <param name="blood_type">user's blood type.</param>
    /// <param name="emergency_contact">user's emergency contact.</param>
    /// <param name="weight">user's weigth.</param>
    /// <param name="height">user's heigth.</param>
    public class UserCreateAccountModel(string username, string password, string mail, string name, DateOnly birth_date, string cnp, BloodType blood_type, string emergency_contact, double weight, int height)
    {
        /// <summary>
        /// Gets username.
        /// </summary>
        public string username { get; private set; } = username;

        /// <summary>
        /// Gets password.
        /// </summary>
        public string password { get; private set; } = password;

        /// <summary>
        /// Gets Mail.
        /// </summary>
        public string mail { get; private set; } = mail;

        /// <summary>
        /// Gets name.
        /// </summary>
        public string name { get; private set; } = name;

        /// <summary>
        /// Gets birth_date.
        /// </summary>
        public DateOnly birthDate { get; private set; } = birth_date;

        /// <summary>
        /// Gets cnp.
        /// </summary>
        public string cnp { get; private set; } = cnp;

        /// <summary>
        /// Gets Blood type.
        /// </summary>
        public BloodType bloodType { get; private set; } = blood_type;

        /// <summary>
        /// Gets emergency contact.
        /// </summary>
        public string emergencyContact { get; private set; } = emergency_contact;

        /// <summary>
        /// Gets user's weigth.
        /// </summary>
        public double weight { get; private set; } = weight;

        /// <summary>
        /// Gets user's heigth.
        /// </summary>
        public int height { get; private set; } = height;

    }
}