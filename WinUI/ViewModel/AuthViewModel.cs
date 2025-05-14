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
    /// <param name="user_service_model">Servuce for Login or Create Account.</param>
    public class AuthViewModel(IAuthService user_service_model)
    {
        /// <summary>
        /// Gets the Service (Model) for the user.
        /// </summary>
        public IAuthService authService { get; private set; } = user_service_model;

        /// <summary>
        /// Logs the user in if the user exists and the password for the account is correct.
        /// </summary>
        /// <param name="username">The user's username (from input).</param>
        /// <param name="password">the user's password (from input).</param>
        /// <returns>.</returns>
        /// <exception cref="AuthenticationException">Checks if the user exists and if the password is correct / valid. If not 
        /// it throws an exception.</exception>
        public async Task login(string username, string password)
        {
            bool check_if_user_exists = await this.authService.loadUserByUsername(username);

            if (!check_if_user_exists)
            {
                throw new AuthenticationException("username doesn't exist!");
            }

            bool is_the_password_valid = await this.authService.verifyPassword(password);

            if (!is_the_password_valid)
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
            await this.authService.logOut();
        }

        /// <summary>
        /// Creates an accout for the user.
        /// </summary>
        /// <param name="model_for_creating_user_account">The user's information Model given as UserCreateAccountModel.</param>
        /// <returns>.</returns>
        public async Task createAccount(UserCreateAccountModel model_for_creating_user_account)
        {
            await this.authService.createAccount(model_for_creating_user_account);
        }

        /// <summary>
        /// Gets the user's role.
        /// </summary>
        /// <returns>user's role.</returns>
        public string getUserRole()
        {
            return user_service_model.allUserInformation.role;
        }
    }
}
