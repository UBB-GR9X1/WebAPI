using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Model
{
    /// <summary>
    /// Blood types.
    /// </summary>
    public enum BloodType
    {
        /// <summary>
        /// A+ blood type.
        /// </summary>
        A_POSITIVE,    // A+

        /// <summary>
        /// A- blood type.
        /// </summary>
        A_NEGATIVE,    // A-

        /// <summary>
        /// B+ blood type.
        /// </summary>
        B_POSITIVE,    // B+

        /// <summary>
        /// B- blood type.
        /// </summary>
        B_NEGATIVE,    // B-

        /// <summary>
        /// AB+ blood type.
        /// </summary>
        AB_POSITIVE,   // AB+

        /// <summary>
        /// AB- blood type.
        /// </summary>
        AB_NEGATIVE,   // AB-

        /// <summary>
        /// O+ blood type.
        /// </summary>
        O_POSITIVE,    // O+

        /// <summary>
        /// O- blood type.
        /// </summary>
        O_NEGATIVE, // O-
    }

    static class BloodTypeMethods
    {
        public static String convertToString(this BloodType blood_type)
        {
            switch (blood_type)
            {
                case BloodType.A_POSITIVE:
                    return "A+";
                case BloodType.A_NEGATIVE:
                    return "A-";
                case BloodType.B_POSITIVE:
                    return "B+";
                case BloodType.B_NEGATIVE:
                    return "B-";
                case BloodType.AB_POSITIVE:
                    return "AB+";
                case BloodType.AB_NEGATIVE:
                    return "AB-";
                case BloodType.O_POSITIVE:
                    return "O+";
                case BloodType.O_NEGATIVE:
                    return "O-";
                default:
                    return "UNDEFINED"; // UNREACHABLE BUT C# IS RETARDED
            }
        }
    }

    /// <summary>
    /// User Model for Creating an Account.
    /// <param name="_user_id">user id.</param>
    /// <param name="_username"> user username.</param>
    /// <param name="_password">user's password.</param>
    /// <param name="_mail">user's mail.</param>
    /// <param name="_role">user's role.</param>
    /// </summary>
    public class UserAuthModel(int _user_id, string _username, string _password, string _mail, string _role)
    {
        /// <summary>
        /// Create a default user (no ).
        /// </summary>
        public static readonly UserAuthModel s_dafault = new UserAuthModel(0, "Guest", string.Empty, string.Empty, "User");

        /// <summary>
        /// Gets User ID.
        /// </summary>
        public int user_id { get; private set; } = _user_id;


        /// <summary>
        /// Gets user's username.
        /// </summary>
        public string username { get; private set; } = _username;

        /// <summary>
        /// Gets user's password.
        /// </summary>
        public string password { get; private set; } = _password;

        /// <summary>
        /// Gets user's Mail.
        /// </summary>
        public string mail { get; private set; } = _mail;

        /// <summary>
        /// Gets user's Role.
        /// </summary>
        public string role { get; private set; } = _role;


        /// <summary>
        /// Turn the user Model with the user's information into a string.
        /// </summary>
        /// <returns>a string with the user's informstion</returns>
        public override string ToString()
        {
            return this.user_id + this.username + this.password + this.mail + this.role;
        }
    }
}
