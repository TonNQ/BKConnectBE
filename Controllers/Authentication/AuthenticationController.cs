using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BKConnectBE.Model.Entities;
using BKConnectBE.Model.Dtos;
using BKConnect.Service;
using BKConnectBE.Service;
using BKConnect.BKConnectBE.Common;
using System.Security.Claims;
using BKConnect.Controllers;
using BKConnectBE.Model.Dtos.Authentications;

namespace WebApplication1.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        
        public AuthenticationController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
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
                RefreshToken refreshToken = await _jwtService.GenerateRefreshTokenAsync(userInfo.Id, userInfo.Name, userInfo.Role);
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

        [HttpPost("validate")]
        public async Task<ActionResult<Responses>> ValidateAccessToken(string accessToken)
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
