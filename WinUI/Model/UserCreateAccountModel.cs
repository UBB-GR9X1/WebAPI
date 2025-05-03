using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{

    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreateAccountModel"/> class.
    /// </summary>
    /// <param name="_username">username.</param>
    /// <param name="_password">user password.</param>
    /// <param name="_mail">user mail.</param>
    /// <param name="_name">user's name.</param>
    /// <param name="_birth_date">user's birthdate.</param>
    /// <param name="_cnp">user's cnp.</param>
    /// <param name="_blood_type">user's blood type.</param>
    /// <param name="_emergency_contact">user's emergency contact.</param>
    /// <param name="_weight">user's weigth.</param>
    /// <param name="_height">user's heigth.</param>
    public class UserCreateAccountModel(string _username, string _password, string _mail, string _name, DateOnly _birth_date, string _cnp, BloodType _blood_type, string _emergency_contact, double _weight, int _height)
    {
        /// <summary>
        /// Gets username.
        /// </summary>
        public string username { get; private set; } = _username;

        /// <summary>
        /// Gets password.
        /// </summary>
        public string password { get; private set; } = _password;

        /// <summary>
        /// Gets Mail.
        /// </summary>
        public string mail { get; private set; } = _mail;

        /// <summary>
        /// Gets Name.
        /// </summary>
        public string name { get; private set; } = _name;

        /// <summary>
        /// Gets BirthDate.
        /// </summary>
        public DateOnly birth_date { get; private set; } = _birth_date;

        /// <summary>
        /// Gets CNP.
        /// </summary>
        public string cnp { get; private set; } = _cnp;

        /// <summary>
        /// Gets Blood type.
        /// </summary>
        public BloodType blood_type { get; private set; } = _blood_type;

        /// <summary>
        /// Gets emergency contact.
        /// </summary>
        public string emergency_contact { get; private set; } = _emergency_contact;

        /// <summary>
        /// Gets user's weigth.
        /// </summary>
        public double weight { get; private set; } = _weight;

        /// <summary>
        /// Gets user's heigth.
        /// </summary>
        public int height { get; private set; } = _height;

    }
}