using Oratoria.Persistence.Entities;

namespace Oratoria.Persistence.Services
{
    public interface IUserService
    {
        public Task<bool> AddUser(string name, string login, string password, int roleId);
        public Task<UserEntity?> GetUser(string login, string password);
        public Task<bool> ValidateUserByLogin(string login);
        public Task<bool> ChangePassword(string login, string newPassword);
        public Task<bool> Delete(Guid id);
    }
}
