using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Model;

namespace ClassLibrary.Repository
{
    /// <summary>
    /// Interface for LogInRepository which does the following things:
    /// Makes the connection with the database in order to get information about the user
    /// useful for the login and for creating a new account.
    /// </summary>
    public interface ILogInRepository
    {
        /// <summary>
        /// Checks the action the user makes, loging in or loging out and adds it to the database.
        /// </summary>
        /// <param name="user_id">The id (unique) of the user we are checking.</param>
        /// <param name="action_type_login_or_logout">The acction the user makes: loging in / loging out.</param>
        /// <returns> 1 of the rows were modified.</returns>
        /// <exception cref="AuthenticationException">Throws exception if the type was not valid or if 
        /// there was a logger action error.</exception>
        Task<bool> authenticationLogService(int user_id, ActionType action_type_login_or_logout);

        /// <summary>
        /// Creates a user account with the given information and adds it to the database.
        /// </summary>
        /// <param name="model_for_creating_user_account">The "model" for creating an account - domain.</param>
        /// <returns> 1 if the user account was created, 0 otherwise.</returns>
        /// <exception cref="AuthenticationException">Throws an exception if the user already exists
        /// or if there was a database error.</exception>
        Task<bool> createAccount(UserCreateAccountModel model_for_creating_user_account);

        /// <summary>
        /// Gets a user's information from the database based on the username.
        /// </summary>
        /// <param name="username">The username of the user we are searching for.</param>
        /// <returns>The user of type UserAuthModel.</returns>
        /// <exception cref="AuthenticationException">Exception in case the username was not found in the table.</exception>
        Task<UserAuthModel> getUserByUsername(string username);
    }
}
