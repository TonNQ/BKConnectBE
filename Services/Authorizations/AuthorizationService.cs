using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnect.Common;
using BKConnect.Service.Jwt;
using BKConnectBE.Common;
using BKConnectBE.Model.Dtos.Authorizations;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository.Users;
using BKConnectBE.Services.Email;

namespace BKConnectBE.Services.Authorizations
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationService(
            IUserRepository userRepository,
            IEmailService emailService,
            IJwtService jwtService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _configurationProvider = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
            _mapper = new Mapper(_configurationProvider);
            _emailService = emailService;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Responses> Register(RegisterDto registerDto)
        {
            var responseInfo = new Responses();
            if (await _userRepository.GetByEmailAsync(registerDto.Email) is not null)
            {
                responseInfo.Code = ResponseCode.HAVE_ERROR;
                responseInfo.Message = MsgNo.ERROR_EMAIL_HAS_USED;
                return responseInfo;
            }

            var user = _mapper.Map<User>(registerDto);
            await _userRepository.CreateUserAsync(user);
            responseInfo.Message = MsgNo.SUCCESS_REGISTER;
            responseInfo.Data = new
            {
                UserId = user.Id
            };

            await SendActiveAccountEmail(user);
            return responseInfo;
        }

        public async Task<Responses> ActiveAccount(string secretHash)
        {
            var responseInfo = new Responses();

            var data = _jwtService.DecodeToken(secretHash);

            if (!data.ContainsKey("UserId"))
            {
                responseInfo.Code = ResponseCode.HAVE_ERROR;
                responseInfo.Message = MsgNo.ERROR_TOKEN_INVALID;
                return responseInfo;
            }

            var user = await _userRepository.GetByIdAsync(data["UserId"]);

            if (user is null)
            {
                responseInfo.Code = ResponseCode.NOT_FOUND;
                responseInfo.Message = MsgNo.ERROR_USER_NOTFOUND;
                return responseInfo;
            }

            if (user.IsActive)
            {
                responseInfo.Code = ResponseCode.HAVE_ERROR;
                responseInfo.Message = MsgNo.ERROR_USER_HAD_ACTIVED;
                return responseInfo;
            }
            
            user.IsActive = true;
            await _userRepository.SaveChangeUserAsync();

            responseInfo.Message = MsgNo.SUCCESS_ACTIVE_ACCOUNT;
            responseInfo.Data = new
            {
                UserId = user.Id
            };
            return responseInfo;
        }

        private async Task SendActiveAccountEmail(User user)
        {
            var receiver = user.Email;
            var subject = "[BKConnect] Xác nhận tài khoản";
            var token = _jwtService.GenerateAccessToken(user.Id, user.Name, user.RoleId);

            var linkActive = $"{Helper.GetDomainName(_httpContextAccessor)}/author/active-account?secretHash={token}";

            var emailMessage = _emailService.MessageEmailForActiveAccount(
                    _emailService.ConvertHtmlToString("Templates/Emails/ActiveAccount.html"), receiver, linkActive);
            await _emailService.SendEmailAsync(receiver, subject, emailMessage);
        }
    }
}