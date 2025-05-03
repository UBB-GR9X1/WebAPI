using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;

namespace WinUI.Service
{
    public interface IAuthService
    {

        /// <summary>
        /// Gets the user information (given as UserAuthModel).
        /// </summary>
        UserAuthModel all_user_information { get; }

        /// <summary>
        /// Creates an accout, checking for validation errors in the user's inputs.
        /// </summary>
        /// <param name="_model_for_creating_user_account">The user's information Model given as UserCreateAccountModel</param>
        /// <returns>User action: LOGIN if the accout got created, LOGOUT otherwise.</returns>
        /// <exception cref="AuthenticationException">Exceptions if the inputs were not valid
        /// + messages for each validation error.</exception>
        Task<bool> createAccount(UserCreateAccountModel _model_for_creating_user_account);

        /// <summary>
        /// Loads the user page based on the username.
        /// </summary>
        /// <param name="_username">The user's username</param>
        /// <returns>true, no mather what.</returns>
        Task<bool> loadUserByUsername(string _username);

        /// <summary>
        /// Sets the user's action.
        /// </summary>
        /// <param name="_action_type_login_or_logout">The type of the user's action.</param>
        /// <returns>The action setter.</returns>
        Task<bool> logAction(ActionType _action_type_login_or_logout);

        /// <summary>
        /// Logs out the user from the application (goes back to Main Window).
        /// </summary>
        /// <returns>the result of the logging out</returns>
        /// <exception cref="AuthenticationException">Checks if the user is logged in, throws an exception if not.</exception>
        Task<bool> logOut();

        /// <summary>
        /// Checks if the password matches at log in (if the user typed the right password).
        /// </summary>
        /// <param name="_user_input_password">The user input password.</param>
        /// <returns>true, if the password matches with the user's one, or false if the one from the input does not match.</returns>
        Task<bool> verifyPassword(string _user_input_password);
    }
}
