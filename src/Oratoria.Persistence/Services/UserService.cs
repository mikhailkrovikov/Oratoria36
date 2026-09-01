using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oratoria.Persistence.Entities;
using System.Security.Cryptography;

namespace Oratoria.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly AppDBContext _dbContext;
        private readonly ILogger _logger;

        public UserService(AppDBContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddUser(string name, string login, string password, int roleId)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Login.Equals(login));
                if (user != null)
                    return false;

                _dbContext.Users.Add(new UserEntity
                {
                    Name = name,
                    Login = login,
                    Password = HashPassword(password),
                    RoleId = roleId
                });
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) return false;
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        public async Task<UserEntity?> GetUser(string login, string password)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Login == login);
            if (user is null)
                return null;
            return VerifyPassword(password, user.Password) ? user : null;
        }

        public async Task<bool> ValidateUserByLogin(string login)
        {
            try
            {
                var user = await _dbContext.Users
                 .FirstOrDefaultAsync(u => u.Login == login);
                if (user == null) return true;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        public async Task<bool> ChangePassword(string login, string newPaswword)
        {
            try
            {
                var user = await _dbContext.Users
               .FirstOrDefaultAsync(u => u.Login == login);
                if (user == null) return false;
                user.Password = HashPassword(newPaswword);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        private static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100000,
                16));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        private static bool VerifyPassword(string inputPassword, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(inputPassword) || string.IsNullOrWhiteSpace(storedHash))
                return false;
            var parts = storedHash.Split('.');
            if (parts.Length != 2)
                return false;
            byte[] salt;
            byte[] expectedHash;
            salt = Convert.FromBase64String(parts[0]);
            expectedHash = Convert.FromBase64String(parts[1]);
            byte[] actualHash = KeyDerivation.Pbkdf2(
                inputPassword,
                salt,
                KeyDerivationPrf.HMACSHA256,
                100000,
                expectedHash.Length);
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }


    }
}
