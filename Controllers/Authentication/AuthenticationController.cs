using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnect.Service.Jwt;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Dtos.Authentication;
using BKConnectBE.Service.Authentication;
using BKConnectBE.Service.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BKConnectBE.Model.Dtos.RefreshTokenManagement;

namespace BKConnectBE.Controllers.Authentication
{
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _AuthenticationService;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(IAuthenticationService AuthenticationService,
            IJwtService jwtService,
            IUserService userService,
            IHttpContextAccessor httpContextAccessor)
        {
            _AuthenticationService = AuthenticationService;
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
                var responseInfo = await _AuthenticationService.Register(AccountDto);
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
                await _AuthenticationService.ActiveAccount(secretHash);
                return Redirect("localhost:5173/active_account");
            }
            catch (Exception)
            {
                return Redirect("https://fb.com");
            }
        }

        [AllowAnonymous]
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

                string token = _jwtService.GenerateAccessToken(userInfo.Id, userInfo.Name, userInfo.Role);
                RefreshTokenDto refreshToken = await _jwtService.GenerateRefreshTokenAsync(userInfo.Id, userInfo.Name, userInfo.Role);
                StoreToken(refreshToken.Token);

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
        
        [HttpGet("validate")]
        public async Task<ActionResult<Responses>> ValidateAccessToken([FromHeader(Name = "Authorization")] string accessToken)
        {
            try
            {
                string id = _jwtService.ValidateToken(true, accessToken);
                UserDto userInfo = await _userService.GetByIdAsync(id);

                return this.Success(userInfo, MsgNo.SUCCESS_LOGIN);
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

                var response = await _AuthenticationService.ForgotPassword(input.Email);
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
                var response = await _AuthenticationService.ResetPassword(resetPassword);
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
                var response = await _AuthenticationService.CheckToken(secretHash);
                return this.Success(response.Data, response.Message);
            }
            catch (Exception)
            {
                return new UnprocessableEntityResult();
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
