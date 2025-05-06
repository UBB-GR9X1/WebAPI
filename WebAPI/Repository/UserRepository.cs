using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using ClassLibrary.Domain;
using Entity;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.IRepository;

namespace WebApi.Repository
{
    /// <summary>
    /// Repository class for managing user-related database operations.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<List<User>> getAllUsersAsync()
        {
            var userEntities = await dbContext.Users.ToListAsync();

            return userEntities.Select(u => new User
            {
                userId = u.UserId,
                username = u.Username,
                password = u.Password,
                mail = u.Mail,
                role = u.Role,
                name = u.Name,
                birthDate = u.BirthDate,
                cnp = u.CNP,
                address = u.Address,
                phoneNumber = u.PhoneNumber,
                registrationDate = u.RegistrationDate
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<User> getUserByIdAsync(int id)
        {
            var userEntity = await dbContext.Users.FindAsync(id);

            if (userEntity == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            return new User
            {
                userId = userEntity.UserId,
                username = userEntity.Username,
                password = userEntity.Password,
                mail = userEntity.Mail,
                role = userEntity.Role,
                name = userEntity.Name,
                birthDate = userEntity.BirthDate,
                cnp = userEntity.CNP,
                address = userEntity.Address,
                phoneNumber = userEntity.PhoneNumber,
                registrationDate = userEntity.RegistrationDate
            };
        }

        /// <inheritdoc/>
        public async Task addUserAsync(User user)
        {
            var userEntity = new UserEntity
            {
                Username = user.username,
                Password = user.password,
                Mail = user.mail,
                Role = user.role,
                Name = user.name,
                BirthDate = user.birthDate,
                CNP = user.cnp,
                Address = user.address,
                PhoneNumber = user.phoneNumber,
                RegistrationDate = user.registrationDate
            };

            dbContext.Users.Add(userEntity);
            await dbContext.SaveChangesAsync();

            user.userId = userEntity.UserId;
        }

        /// <inheritdoc/>
        public async Task deleteUserAsync(int id)
        {
            var userEntity = await dbContext.Users.FindAsync(id);

            if (userEntity == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            dbContext.Users.Remove(userEntity);
            await dbContext.SaveChangesAsync();
        }
    }
}
