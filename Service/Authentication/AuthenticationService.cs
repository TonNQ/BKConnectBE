using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnect.Service.Jwt;
using BKConnectBE.Common;
using BKConnectBE.Model.Dtos.Authentication;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Users;
using BKConnectBE.Service.Email;

namespace BKConnectBE.Service.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<User> _genericRepositoryForUser;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public AuthenticationService(
            IUserRepository userRepository,
            IGenericRepository<User> genericRepositoryForUser,
            IEmailService emailService,
            IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _genericRepositoryForUser = genericRepositoryForUser;
            _emailService = emailService;
            _jwtService = jwtService;
            _configurationProvider = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(_configurationProvider);
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Responses> Register(AccountDto AccountDto)
        {
            if (await _userRepository.GetByEmailAsync(AccountDto.Email) is not null)
            {
                throw new Exception(MsgNo.ERROR_EMAIL_HAS_USED);
            }

            var user = _mapper.Map<User>(AccountDto);

            await SendActiveAccountEmail(user);

            await _genericRepositoryForUser.AddAsync(user);
            await _genericRepositoryForUser.SaveChangeAsync();

            var responseInfo = new Responses
            {
                Message = MsgNo.SUCCESS_REGISTER,
                Data = new
                {
                    user_id = user.Id
                }
            };
            return responseInfo;
        }

        public async Task ActiveAccount(string secretHash)
        {

            var data = _jwtService.DecodeToken(secretHash);

            if (!data.ContainsKey("UserId"))
            {
                throw new Exception(MsgNo.ERROR_TOKEN_INVALID);
            }

            var user = await _genericRepositoryForUser.GetByIdAsync(data["UserId"])
                ?? throw new Exception(MsgNo.ERROR_USER_NOT_FOUND);

            if (user.IsActive)
            {
                throw new Exception(MsgNo.ERROR_USER_HAD_ACTIVED);
            }

            user.IsActive = true;
            await _genericRepositoryForUser.SaveChangeAsync();
        }

        private async Task SendActiveAccountEmail(User user)
        {
            try
            {
                var receiver = user.Email;
                var subject = Constants.EMAIL_ACTIVE_ACCOUNT_TITLE;
                var token = _jwtService.GenerateAccessToken(user.Id, user.Name, user.Role);

                var linkActive = $"{Helper.GetDomainName(_httpContextAccessor)}/active-account?secretHash={token}";

                var emailMessage = _emailService.MessageEmailForActiveAccount(
                        _emailService.ConvertHtmlToString("Templates/Emails/ActiveAccount.html"), receiver, linkActive);
                await _emailService.SendEmailAsync(receiver, subject, emailMessage);
            }
            catch (Exception)
            {
                throw new Exception(MsgNo.ERROR_SEND_EMAIL);
            }
        }
    }
}