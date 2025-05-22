using ClassLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Repository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns> A list of users.</returns>
        Task<List<User>> getAllUsersAsync();

        /// <summary>
        /// Gets a user by its unique identifier.
        /// </summary>
        /// <param name="id"> the id of the user.</param>
        /// <returns> The user with the given id.</returns>
        Task<User> getUserByIdAsync(int id);

        /// <summary>
        /// Gets a user by its username.
        /// </summary>
        /// <param name="user"> The user to be searched.</param>
        /// <returns> The user with the given username.</returns>
        Task addUserAsync(User user);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task updateUserAsync(User user);

        /// <summary>
        /// Deletes a user by its unique identifier.
        /// </summary>
        /// <param name="id"> The id of the user</param>
        /// <returns> task representing the asynchronous operation.</returns>

        Task deleteUserAsync(int id);
        /// <summary>
        /// Retrieves all users that match the specified role.
        /// </summary>
        /// <param name="role">The role used to filter users (e.g., "Admin", "User").</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of users with the specified role.</returns>
        Task<List<User>> getUsersByRoleAsync(string role);
       
        /// <summary>
        /// Retrieves all users that match a given name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>A list of users with the specified name.</returns>
        Task<List<User>> getUsersByNameAsync(string name);

        /// <summary>
        /// Retrieves a user by their CNP.
        /// </summary>
        /// <param name="cnp">The CNP of the user to search for.</param>
        /// <returns>The user with the specified CNP.</returns>
        Task<User> getUserByCNPAsync(string cnp);
    }
}
