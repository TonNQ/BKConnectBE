using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Users
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
    }
}