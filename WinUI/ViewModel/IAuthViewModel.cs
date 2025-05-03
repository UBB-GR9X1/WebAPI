using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Model;
using WinUI.Service;

namespace WinUI.ViewModel
{

    /// <summary>
    /// Interface for AuthViewModel:
    /// The view model for Login and Create account.
    /// </summary>
    public interface IAuthViewModel
    {
        /// <summary>
        /// The service / model for creating an account / loging in.
        /// </summary>
        IAuthService auth_service { get; }

        /// <summary>
        /// Creates an accout for the user.
        /// </summary>
        /// <param name="_model_for_creating_user_account">The user's information Model given as UserCreateAccountModel.</param>
        /// <returns>.</returns>
        Task createAccount(UserCreateAccountModel _model_for_creating_user_account);

        /// <summary>
        /// Logs the user in if the user exists and the password for the account is correct.
        /// </summary>
        /// <param name="_username">The user's username (from input).</param>
        /// <param name="_password">the user's password (from input).</param>
        /// <returns>.</returns>
        /// <exception cref="AuthenticationException">Checks if the user exists and if the password is correct / valid. If not 
        /// it throws an exception.</exception>
        Task login(string _username, string _password);

        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns>.</returns>
        Task logout();

        /// <summary>
        /// Gets the user's role.
        /// </summary>
        public string getUserRole();
    }
}
