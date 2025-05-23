using Microsoft.AspNetCore.Mvc;
using Data;
using Entity;
using ClassLibrary.Repository;
using ClassLibrary.Domain;
using System;

namespace Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user_repository;

        public UserController(IUserRepository user_repository)
        {
            this._user_repository = user_repository;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>An ActionResult containing a list of users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<User>>> getAllUsers()
        {
            try
            {
                List<User> users = await this._user_repository.getAllUsersAsync();
                return this.Ok(users);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving users. Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>An ActionResult containing the user.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> getUserById(int id)
        {
            try
            {
                User user = await this._user_repository.getUserByIdAsync(id);
                if (user == null)
                {
                    return this.NotFound($"User with ID {id} was not found.");
                }
                return this.Ok(user);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the user. Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return this.BadRequest("Valid user entity is required.");
            }

            try
            {
                await this._user_repository.addUserAsync(user);
                return this.CreatedAtAction(nameof(getUserById), new { id = user.userId }, user);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating user. Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The updated user data.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> updateUser(int id, [FromBody] User user)
        {
            if (user == null)
            {
                return this.BadRequest("Valid user entity is required.");
            }

            if (id != user.userId)
            {
                return this.BadRequest("User ID in the URL must match the ID in the request body.");
            }

            try
            {
                await this._user_repository.updateUserAsync(user);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"User with ID {id} was not found.");
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating user. Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Updates an existing user identified by name.
        /// </summary>
        /// <param name="name">The name of the user to update.</param>
        /// <param name="user">The updated user data.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpPut("by-name/{name}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUserByName(string name, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User data must be provided.");
            }

            if (!string.Equals(name, user.name, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Name in URL must match the user's name in the request body.");
            }

            try
            {
                // Fetch users by name (assuming your repo returns a list)
                var users = await _user_repository.getUsersByNameAsync(name);
                if (users == null || users.Count == 0)
                {
                    return NotFound($"No user found with name '{name}'.");
                }

                if (users.Count > 1)
                {
                    return BadRequest($"Multiple users found with the name '{name}'. Please update by user ID instead.");
                }

                var existingUser = users[0];

                // Optionally, ensure the userId matches or handle as needed
                user.userId = existingUser.userId;

                await _user_repository.updateUserAsync(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating the user. Error: {ex.Message}");
            }
        }


        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>An ActionResult indicating success or failure.</returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> deleteUser(int id)
        {
            try
            {
                await this._user_repository.deleteUserAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"User with ID {id} was not found.");
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting user. Error: {exception.Message}");
            }
        }


        /// <summary>
        /// Retrieves all users by role.
        /// </summary>
        /// <param name="role">The role to filter users by.</param>
        /// <returns>An ActionResult containing a list of users.</returns>
        [HttpGet("role/{role}")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<User>>> GetUsersByRole(string role)
        {
            try
            {
                List<User> users = await this._user_repository.getUsersByRoleAsync(role);

                if (users == null || users.Count == 0)
                {
                    return this.NotFound($"No users found with role '{role}'.");
                }

                return this.Ok(users);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving users by role. Error: {exception.Message}");
            }
        }
        /// <summary>
        /// Retrieves all users that match a given name.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>List of users with the specified name.</returns>
        [HttpGet("by-name/{name}")]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<User>>> GetUsersByName(string name)
        {
            try
            {
                List<User> users = await this._user_repository.getUsersByNameAsync(name);
                return this.Ok(users);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving users by name. Error: {exception.Message}");
            }
        }

        /// <summary>
        /// Retrieves a user by their CNP.
        /// </summary>
        /// <param name="cnp">The CNP of the user to search for.</param>
        /// <returns>A user with the specified CNP.</returns>
        [HttpGet("by-cnp/{cnp}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> GetUserByCNP(string cnp)
        {
            try
            {
                User user = await this._user_repository.getUserByCNPAsync(cnp);
                if (user == null)
                {
                    return this.NotFound($"User with CNP {cnp} not found.");
                }
                return this.Ok(user);
            }
            catch (Exception exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving user by CNP. Error: {exception.Message}");
            }
        }


    }
}
