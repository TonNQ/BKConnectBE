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

        public async Task<Responses> Register(AccountDto accountDto)
        {
            if (await _userRepository.GetByEmailAsync(accountDto.Email) is not null)
            {
                throw new Exception(MsgNo.ERROR_EMAIL_HAS_USED);
            }

            var user = _mapper.Map<User>(accountDto);

            var token = _jwtService.GenerateAccessToken(user.Id, user.Name, user.Role);
            var linkActive = $"{Helper.GetDomainName(_httpContextAccessor)}/activeAccount?secretHash={token}";
            await SendAuthenticationEmail(user, linkActive, Constants.EMAIL_ACTIVE_ACCOUNT_TITLE, "Templates/Emails/ActiveAccount.html");

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

        public async Task<Responses> ForgotPassword(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email)
                ?? throw new Exception(MsgNo.ERROR_EMAIL_NOT_REGISTERED);

            if (!user.IsActive)
            {
                throw new Exception(MsgNo.ERROR_ACCOUNT_NOT_ACTIVE);
            }

            var temporaryCode = _jwtService.GenerateTemporaryCode(user.Id);
            var linkResetPassword = $"https://localhost:5173/set_new_password?secretHash={temporaryCode}";
            await SendAuthenticationEmail(user, linkResetPassword, Constants.EMAIL_RESET_PASSWORD_TITLE, "Templates/Emails/ResetPassword.html");

            var responseInfo = new Responses
            {
                Message = MsgNo.SUCCESS_EMAIL_FORGOT_PASSWORD,
                Data = new
                {
                    user_id = user.Id
                }
            };
            return responseInfo;
        }

        public async Task<Responses> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var data = _jwtService.DecodeToken(resetPasswordDto.TemporaryCode);

            if (!(data.ContainsKey("UserId") && data.ContainsKey("Date")))
            {
                throw new Exception(MsgNo.ERROR_TOKEN_INVALID);
            }

            var user = await _genericRepositoryForUser.GetByIdAsync(data["UserId"])
                ?? throw new Exception(MsgNo.ERROR_USER_NOT_FOUND);

            if (!user.IsActive)
            {
                throw new Exception(MsgNo.ERROR_ACCOUNT_NOT_ACTIVE);
            }

            if (!data["Date"].Equals(DateTime.Now.ToString("dd-MM-yyyy")))
            {
                throw new Exception(MsgNo.ERROR_TOKEN_INVALID);
            }

            user.Password = Security.CreateMD5(resetPasswordDto.Password);
            await _genericRepositoryForUser.SaveChangeAsync();

            var responseInfo = new Responses
            {
                Message = MsgNo.SUCCESS_RESET_PASSWORD,
                Data = new
                {
                    user_id = user.Id
                }
            };
            return responseInfo;
        }

        public async Task<Responses> CheckToken(string secretHash)
        {
            try
            {
                var data = _jwtService.DecodeToken(secretHash);

                if (!(data.ContainsKey("UserId") && data.ContainsKey("Date")))
                {
                    throw new Exception(MsgNo.ERROR_TOKEN_INVALID);
                }

                var user = await _genericRepositoryForUser.GetByIdAsync(data["UserId"])
                    ?? throw new Exception(MsgNo.ERROR_USER_NOT_FOUND);

                if (!user.IsActive)
                {
                    throw new Exception(MsgNo.ERROR_ACCOUNT_NOT_ACTIVE);
                }

                return new Responses
                {
                    Message = MsgNo.SUCCESS_TOKEN_VALID,
                    Data = new
                    {
                        user_id = user.Id
                    }
                };
            }
            catch
            {
                throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
            }
        }

        private async Task SendAuthenticationEmail(User user, string link, string title, string emailHtml)
        {
            try
            {
                var receiver = user.Email;
                var subject = title;

                var emailMessage = _emailService.MessageEmailForActiveAccount(
                        _emailService.ConvertHtmlToString(emailHtml), receiver, link);
                await _emailService.SendEmailAsync(receiver, subject, emailMessage);
            }
            catch (Exception)
            {
                throw new Exception(MsgNo.ERROR_SEND_EMAIL);
            }
        }
    }
}