using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Dtos.Authentication;
using BKConnectBE.Model.Dtos.Parameters;

namespace BKConnectBE.Service.Users
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(AccountDto account);
        Task<UserDto> GetByIdAsync(string userId);
        Task<List<UserSearchDto>> SearchListOfUsers(string userId, SearchKeyConditionWithPage searchCondition);
        Task<UserDto> UpdateUserAsync(string userId, UserInputDto user);
        Task ChangePasswordAsync(string userId, ChangePasswordDto password);
    }
}