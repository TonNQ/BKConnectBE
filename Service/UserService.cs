using BKConnect.Model.Dtos.User;
using BKConnectBE.Model.Dtos;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;

namespace BKConnect.Service;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;
    public UserService(IGenericRepository<User> userRepository) 
    {
        _userRepository = userRepository;
    }
    public async Task<UserDto> GetUserByIdAsync(string id)
    {
        var u = await _userRepository.GetByIdAsync(id);
        if (u == null) return null;
        return new UserDto(u);
    }

    public async Task UpdateUserAsync(UpdateUserDto userDto)
    {
        var u = await _userRepository.GetByIdAsync(userDto.UserId);
        if (u == null) throw new Exception("UserNotFound");
        u.Name = userDto.Name ?? u.Name;
        _userRepository.Update(u);
        await _userRepository.SaveAsync();
    }
}
