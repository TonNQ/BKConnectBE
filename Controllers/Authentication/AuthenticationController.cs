using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnect.Service.Jwt;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Dtos.Authentication;
using BKConnectBE.Service.Authentication;
using BKConnectBE.Service.Users;
using Microsoft.AspNetCore.Mvc;
using BKConnectBE.Model.Dtos.RefreshTokenManagement;
using BKConnectBE.Common.Attributes;

namespace BKConnectBE.Controllers.Authentication
{
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(IAuthenticationService authenticationService,
            IJwtService jwtService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _authenticationService = authenticationService;
            _jwtService = jwtService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Responses>> Register(AccountDto AccountDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(MsgNo.ERROR_INPUT_INVALID);
                }
                var responseInfo = await _authenticationService.Register(AccountDto);
                return this.Success(responseInfo.Data, responseInfo.Message);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("activeAccount")]
        public async Task<IActionResult> ActiveAccount(string secretHash)
        {
            try
            {
                await _authenticationService.ActiveAccount(secretHash);
                return Redirect("localhost:5173/active_account");
            }
            catch (Exception)
            {
                return Redirect("https://fb.com");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Responses>> Login(AccountDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(this.Error(MsgNo.ERROR_INPUT_INVALID));
                }

                UserDto userInfo = await _userService.GetUserAsync(request);

                RefreshTokenDto refreshToken = await _jwtService.GenerateRefreshTokenAsync(userInfo.Id, userInfo.Name, userInfo.Role);
                StoreToken(refreshToken.Token);
                string token = _jwtService.GenerateAccessToken(userInfo.Id, userInfo.Name, userInfo.Role, refreshToken.Id);

                var data = new
                {
                    access_token = token,
                    user = userInfo
                };

                return this.Success(data, MsgNo.SUCCESS_LOGIN);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getToken")]
        public ActionResult<Responses> GetNewAccessToken()
        {
            try
            {
                string refreshToken = Request.Cookies["refresh_token"];
                string id = _jwtService.ValidateToken(false, refreshToken);
                string newAccessToken = _jwtService.GetNewAccessToken(refreshToken);

                var data = new
                {
                    access_token = newAccessToken,
                    user_id = id
                };

                return this.Success(data, MsgNo.SUCCESS_GET_TOKEN);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [CustomAuthorize]
        [HttpGet("validate")]
        public async Task<ActionResult<Responses>> ValidateAccessToken()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    UserDto userInfo = await _userService.GetByIdAsync(userId);
                    return this.Success(userInfo, MsgNo.SUCCESS_LOGIN);
                }
                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult<Responses>> ForgotPassword(EmailDto input)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(this.Error(MsgNo.ERROR_INPUT_INVALID));
                }

                var response = await _authenticationService.ForgotPassword(input.Email);
                return this.Success(response.Data, response.Message);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult<Responses>> ResetPassword(ResetPasswordDto resetPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(this.Error(MsgNo.ERROR_INPUT_INVALID));
                }
                var response = await _authenticationService.ResetPassword(resetPassword);
                return this.Success(response.Data, response.Message);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPost("checkToken")]
        public async Task<ActionResult<Responses>> CheckToken(string secretHash)
        {
            try
            {
                var response = await _authenticationService.CheckToken(secretHash);
                return this.Success(response.Data, response.Message);
            }
            catch (Exception)
            {
                return new UnprocessableEntityResult();
            }
        }

        [CustomAuthorize]
        [HttpPost("logout")]
        public async Task<ActionResult<Responses>> Logout()
        {
            try
            {
                string token = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
                var response = await _authenticationService.Logout(token);
                _httpContextAccessor.HttpContext.Request.Headers.Authorization = "";
                return this.Success(response.Data, response.Message);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        private void StoreToken(string storedToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            };
            string token = Request.Cookies["refresh_token"];
            if (token != null)
            {
                HttpContext.Response.Cookies.Delete("refresh_token");
            }

            HttpContext.Response.Cookies.Append("refresh_token", storedToken, cookieOptions);
        }
    }
}
