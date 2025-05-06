using Microsoft.AspNetCore.Mvc;
using Data;
using Entity;
using ClassLibrary.IRepository;
using ClassLibrary.Domain;

namespace Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository _userRepository)
        {
            this.userRepository = _userRepository;
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>An ActionResult containing a list of users.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            try
            {
                List<User> users = await this.userRepository.getAllUsersAsync();
                return this.Ok(users);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving users. Error: {ex.Message}");
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
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                User user = await this.userRepository.getUserByIdAsync(id);
                if (user == null)
                {
                    return this.NotFound($"User with ID {id} was not found.");
                }
                return this.Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving the user. Error: {ex.Message}");
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
                await this.userRepository.addUserAsync(user);
                return this.CreatedAtAction(nameof(GetUserById), new { id = user.userId }, user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while creating user. Error: {ex.Message}");
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
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await this.userRepository.deleteUserAsync(id);
                return this.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound($"User with ID {id} was not found.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting user. Error: {ex.Message}");
            }
        }
    }
}
