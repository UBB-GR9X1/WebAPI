using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Enum
{
    public enum ActionType
    {
        /// <summary>
        /// User login action.
        /// </summary>
        LOGIN,

        /// <summary>
        /// User logout action.
        /// </summary>
        LOGOUT,

        /// <summary>
        /// Profile update action.
        /// </summary>
        UPDATE_PROFILE,

        /// <summary>
        /// password change action.
        /// </summary>
        CHANGE_PASSWORD,

        /// <summary>
        /// Account deletion action.
        /// </summary>
        DELETE_ACCOUNT,

        /// <summary>
        /// Account creation action.
        /// </summary>
        CREATE_ACCOUNT,
    }
    public static class ActionTypeMethods
    {
        public static String convertToString(this ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.LOGIN:
                    return "LOGIN";
                case ActionType.LOGOUT:
                    return "LOGOUT";
                case ActionType.CREATE_ACCOUNT:
                    return "CREATE_ACCOUNT";
                default:
                    return "UNDEFINED";
            }
        }
    }
}
