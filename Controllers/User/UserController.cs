using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnect.Service.Jwt;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Service.Users;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.User
{
    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        public UserController(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpGet("getProfile")]
        public async Task<ActionResult<Responses>> GetUserProfile([FromHeader(Name = "Authorization")]string accessToken)
        {
            try
            {
                string id = _jwtService.ValidateToken(true, accessToken);

                Dictionary<string, string> tokenInfo = _jwtService.DecodeToken(accessToken);
                UserDto userInfo = await _userService.GetByIdAsync(tokenInfo["UserId"]);

                return this.Success(userInfo, MsgNo.SUCCESS_GET_PROFILE);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPut("updateProfile")]
        public async Task<ActionResult<Responses>> UpdateUserProfile([FromHeader(Name = "Authorization")]string accessToken, UserInputDto userDto)
        {
            try
            {
                string id = _jwtService.ValidateToken(true, accessToken);

                if (!ModelState.IsValid)
                {
                    return BadRequest(this.Error(MsgNo.ERROR_INPUT_INVALID));
                }

                UserDto userInfo = await _userService.UpdateUserAsync(id, userDto);

                return this.Success(userInfo, MsgNo.SUCCESS_UPDATE_PROFILE);
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPut("changePassword")]
        public async Task<ActionResult<Responses>> ChangePassword([FromHeader(Name = "Authorization")]string accessToken, PasswordDto passwordDto)
        {
            try
            {
                string id = _jwtService.ValidateToken(true, accessToken);

                if (!ModelState.IsValid)
                {
                    return BadRequest(this.Error(MsgNo.ERROR_INPUT_INVALID));
                }

                await _userService.ChangePasswordAsync(id, passwordDto);

                return this.Success(id, MsgNo.SUCCESS_UPDATE_PROFILE);
            }
            catch(Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }
    }
}