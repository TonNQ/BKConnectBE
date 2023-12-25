using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Users
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(string id);
        Task<List<User>> GetListUsersByListIdAsync(List<string> ids);
        Task<List<UserSearchDto>> SearchListOfUsers(string userId, SearchKeyConditionWithPage searchCondition);
        Task UpdateUserAsync(User user);
        Task ChangePasswordAsync(string userId, string password);
        Task UpdateLastOnlineAsync(string userId);
        Task<string> GetUsernameById(string userId);
        Task<string> GetAvatarById(string userId);
        Task<bool> GetUserGenderById(string userId);
        Task<bool> IsLecturer(string userId);
        Task UpdateAvatar(string userId, string img);
    }
}