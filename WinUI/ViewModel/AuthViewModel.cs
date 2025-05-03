using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI.Exceptions;
using WinUI.Model;
using WinUI.Service;

namespace WinUI.ViewModel
{
    /// <summary>
    /// The view model for Login and Create account.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AuthViewModel"/> class.
    /// </remarks>
    /// <param name="_user_service_model">Servuce for Login or Create Account.</param>
    public class AuthViewModel(IAuthService _user_service_model) : IAuthViewModel
    {
        /// <summary>
        /// Gets the Service (Model) for the user.
        /// </summary>
        public IAuthService auth_service { get; private set; } = _user_service_model;

        /// <summary>
        /// Logs the user in if the user exists and the password for the account is correct.
        /// </summary>
        /// <param name="_username">The user's username (from input).</param>
        /// <param name="_password">the user's password (from input).</param>
        /// <returns>.</returns>
        /// <exception cref="AuthenticationException">Checks if the user exists and if the password is correct / valid. If not 
        /// it throws an exception.</exception>
        public async Task login(string _username, string _password)
        {
            bool _check_if_user_exists = await this.auth_service.loadUserByUsername(_username);

            if (!_check_if_user_exists)
            {
                throw new AuthenticationException("Username doesn't exist!");
            }

            bool _is_the_password_valid = await this.auth_service.verifyPassword(_password);

            if (!_is_the_password_valid)
            {
                throw new AuthenticationException("Invalid password!");
            }
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns>.</returns>
        public async Task logout()
        {
            await this.auth_service.logOut();
        }

        /// <summary>
        /// Creates an accout for the user.
        /// </summary>
        /// <param name="_model_for_creating_user_account">The user's information Model given as UserCreateAccountModel.</param>
        /// <returns>.</returns>
        public async Task createAccount(UserCreateAccountModel _model_for_creating_user_account)
        {
            await this.auth_service.createAccount(_model_for_creating_user_account);
        }

        /// <summary>
        /// Gets the user's role.
        /// </summary>
        /// <returns>user's role.</returns>
        public string getUserRole()
        {
            return _user_service_model.all_user_information.role;
        }
    }
}
