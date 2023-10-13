using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Service.Users;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.User
{
    [CustomAuthorize]
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getProfile")]
        public async Task<ActionResult<Responses>> GetUserProfile()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    UserDto userInfo = await _userService.GetByIdAsync(userId);
                    return this.Success(userInfo, MsgNo.SUCCESS_GET_PROFILE);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPut("updateProfile")]
        public async Task<ActionResult<Responses>> UpdateUserProfile(UserInputDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(this.Error(MsgNo.ERROR_INPUT_INVALID));
                }

                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    UserDto userInfo = await _userService.UpdateUserAsync(userId, userDto);
                    return this.Success(userInfo, MsgNo.SUCCESS_UPDATE_PROFILE);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPut("changePassword")]
        public async Task<ActionResult<Responses>> ChangePassword(ChangePasswordDto passwordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(this.Error(MsgNo.ERROR_INPUT_INVALID));
                }

                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    await _userService.ChangePasswordAsync(userId, passwordDto);
                    return this.Success(userId, MsgNo.SUCCESS_UPDATE_PROFILE);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }
    }
}