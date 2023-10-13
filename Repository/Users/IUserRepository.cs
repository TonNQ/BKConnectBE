using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Users
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(string id);
        Task<List<UserSearchDto>> SearchListOfUsers(SearchKeyConditionWithPage searchCondition);
        Task<User> UpdateUserAsync(User user);
        Task ChangePasswordAsync(string userId, string password);
    }
}