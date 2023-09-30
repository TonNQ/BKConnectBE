using BKConnect.Model.Dtos.User;
using BKConnectBE.Model.Dtos;

namespace BKConnect.Service;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(string id);

    Task UpdateUserAsync(UpdateUserDto user);
}
