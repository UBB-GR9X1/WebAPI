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
        private readonly ApplicationDbContext _db_context;

        public UserRepository(ApplicationDbContext db_context)
        {
            this._db_context = db_context;
        }

        /// <inheritdoc/>
        public async Task<List<User>> getAllUsersAsync()
        {
            List<UserEntity> user_entities = await _db_context.Users.ToListAsync();

            return user_entities.Select(user => new User
            {
                userId = user.userId,
                username = user.username,
                password = user.password,
                mail = user.mail,
                role = user.role,
                name = user.name,
                birthDate = user.birthDate,
                cnp = user.cnp,
                address = user.address,
                phoneNumber = user.phoneNumber,
                registrationDate = user.registrationDate
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<User> getUserByIdAsync(int id)
        {
            UserEntity user_entity = await _db_context.Users.FindAsync(id);

            if (user_entity == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            return new User
            {
                userId = user_entity.userId,
                username = user_entity.username,
                password = user_entity.password,
                mail = user_entity.mail,
                role = user_entity.role,
                name = user_entity.name,
                birthDate = user_entity.birthDate,
                cnp = user_entity.cnp,
                address = user_entity.address,
                phoneNumber = user_entity.phoneNumber,
                registrationDate = user_entity.registrationDate
            };
        }

        /// <inheritdoc/>
        public async Task addUserAsync(User user)
        {
            UserEntity user_entity = new UserEntity
            {
                username = user.username,
                password = user.password,
                mail = user.mail,
                role = user.role,
                name = user.name,
                birthDate = user.birthDate,
                cnp = user.cnp,
                address = user.address,
                phoneNumber = user.phoneNumber,
                registrationDate = user.registrationDate
            };

            _db_context.Users.Add(user_entity);
            await _db_context.SaveChangesAsync();

            user.userId = user_entity.userId;
        }

        /// <inheritdoc/>
        public async Task updateUserAsync(User user)
        {
            UserEntity user_entity = await _db_context.Users.FindAsync(user.userId);

            if (user_entity == null)
            {
                throw new KeyNotFoundException($"User with ID {user.userId} not found.");
            }

            user_entity.name = user.name;
            user_entity.password = user.password;
            user_entity.address = user.address;
            user_entity.phoneNumber = user.phoneNumber;
            user_entity.birthDate = user.birthDate;

            await _db_context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task deleteUserAsync(int id)
        {
            UserEntity user_entity = await _db_context.Users.FindAsync(id);

            if (user_entity == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }

            _db_context.Users.Remove(user_entity);
            await _db_context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<List<User>> getUsersByRoleAsync(string role)
        {
            List<UserEntity> userEntities = await _db_context.Users
                .Where(user => user.role.ToLower() == role.ToLower())
                .ToListAsync();

            return userEntities.Select(user => new User
            {
                userId = user.userId,
                username = user.username,
                password = user.password,
                mail = user.mail,
                role = user.role,
                name = user.name,
                birthDate = user.birthDate,
                cnp = user.cnp,
                address = user.address,
                phoneNumber = user.phoneNumber,
                registrationDate = user.registrationDate
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<List<User>> getUsersByNameAsync(string name)
        {
            List<UserEntity> userEntities = await _db_context.Users
                .Where(u => u.name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
            return userEntities.Select(user => new User
            {
                userId = user.userId,
                username = user.username,
                password = user.password,
                mail = user.mail,
                role = user.role,
                name = user.name,
                birthDate = user.birthDate,
                cnp = user.cnp,
                address = user.address,
                phoneNumber = user.phoneNumber,
                registrationDate = user.registrationDate
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<User> getUserByCNPAsync(string cnp)
        {
            UserEntity userEntity = await _db_context.Users
                .FirstOrDefaultAsync(user => user.cnp == cnp);
            if (userEntity == null)
            {
                return null;
            }
            return new User
            {
                userId = userEntity.userId,
                username = userEntity.username,
                password = userEntity.password,
                mail = userEntity.mail,
                role = userEntity.role,
                name = userEntity.name,
                birthDate = userEntity.birthDate,
                cnp = userEntity.cnp,
                address = userEntity.address,
                phoneNumber = userEntity.phoneNumber,
                registrationDate = userEntity.registrationDate
            };
        }
    }
}
