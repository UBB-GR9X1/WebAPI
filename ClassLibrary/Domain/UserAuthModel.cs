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
    /// <param name="user_id">user id.</param>
    /// <param name="username"> user username.</param>
    /// <param name="password">user's password.</param>
    /// <param name="mail">user's mail.</param>
    /// <param name="role">user's role.</param>
    /// </summary>
    public class UserAuthModel(int user_id, string username, string password, string mail, string role)
    {
        /// <summary>
        /// Create a default user (no ).
        /// </summary>
        public static readonly UserAuthModel s_dafault = new UserAuthModel(0, "Guest", string.Empty, string.Empty, "User");

        /// <summary>
        /// Gets User ID.
        /// </summary>
        public int userId { get; private set; } = user_id;


        /// <summary>
        /// Gets user's username.
        /// </summary>
        public string username { get; private set; } = username;

        /// <summary>
        /// Gets user's password.
        /// </summary>
        public string password { get; private set; } = password;

        /// <summary>
        /// Gets user's Mail.
        /// </summary>
        public string mail { get; private set; } = mail;

        /// <summary>
        /// Gets user's Role.
        /// </summary>
        public string role { get; private set; } = role;


        /// <summary>
        /// Turn the user Model with the user's information into a string.
        /// </summary>
        /// <returns>a string with the user's informstion</returns>
        public override string ToString()
        {
            return this.userId + this.username + this.password + this.mail + this.role;
        }
    }
}
