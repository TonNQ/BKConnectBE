using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Dtos.Authentication;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Users;
using BKConnectBE.Common;

namespace BKConnectBE.Service.Users
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _genericRepositoryForUser;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IGenericRepository<User> genericRepositoryForUser, IUserRepository userRepository, IMapper mapper)
        {
            _genericRepositoryForUser = genericRepositoryForUser;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserAsync(AccountDto account)
        {
            User user = await _userRepository.GetByEmailAsync(account.Email);
            if (user == null)
            {
                throw new Exception(MsgNo.ERROR_EMAIL_NOT_REGISTERED);
            }
            if (!user.IsActive)
            {
                throw new Exception(MsgNo.ERROR_ACCOUNT_NOT_ACTIVE);
            }
            if (user.Password != Security.CreateMD5(account.Password))
            {
                throw new Exception(MsgNo.ERROR_PASSWORD_WRONG);
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByIdAsync(string userId)
        {
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception(MsgNo.ERROR_USER_NOT_FOUND);
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(string userId, UserInputDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            user.Id = userId;

            if (user == null)
            {
                throw new Exception(MsgNo.ERROR_USER_NOT_FOUND);
            }
            
            User updatedUser =  await _userRepository.UpdateUserAsync(user);
            await _genericRepositoryForUser.SaveChangeAsync();
            return _mapper.Map<UserDto>(updatedUser);
        }

        public async Task ChangePasswordAsync(string userId, PasswordDto password)
        {
            User user = await _genericRepositoryForUser.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception(MsgNo.ERROR_USER_NOT_FOUND);
            }
            if (user.Password != Security.CreateMD5(password.CurrentPassword))
            {
                throw new Exception(MsgNo.ERROR_CURRENT_PASSWORD_WRONG);
            }

            await _userRepository.ChangePasswordAsync(userId, Security.CreateMD5(password.NewPassword));
            await _genericRepositoryForUser.SaveChangeAsync();
        }
    }
}