using ClassLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.IRepository
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
        /// Deletes a user by its unique identifier.
        /// </summary>
        /// <param name="id"> The id of the user</param>
        /// <returns> task representing the asynchronous operation.</returns>
        Task deleteUserAsync(int id);
    }
}
