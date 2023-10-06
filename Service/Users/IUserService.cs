using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Dtos.Authentication;

namespace BKConnectBE.Service.Users
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(AccountDto account);
        Task<UserDto> GetByIdAsync(string userId);
    }
}