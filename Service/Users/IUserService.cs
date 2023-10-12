using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Dtos.Authentication;

namespace BKConnectBE.Service.Users
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(AccountDto account);
        Task<UserDto> GetByIdAsync(string userId);
        Task<UserDto> UpdateUserAsync(string userId, UserInputDto user);
        Task ChangePasswordAsync(string userId, PasswordDto password);
    }
}