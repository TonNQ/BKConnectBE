using BKConnectBE.Model.Dtos;
using BKConnectBE.Model.Dtos.Authentications;

namespace BKConnectBE.Service
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(AccountDto account);
        Task<UserDto> GetByIdAsync(string userId);
    }
}