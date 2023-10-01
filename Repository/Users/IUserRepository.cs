using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Users
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);

        Task CreateUserAsync(User user);

        Task SaveChangeUserAsync();

        Task<User> GetByIdAsync(string id);
    }
}
