using Microsoft.AspNetCore.Mvc;
using BKConnectBE.Repository;
using Microsoft.AspNetCore.Authorization;
using BKConnectBE.Model.Entities;
using BKConnectBE.Model.Dtos;
using BKConnect.Service;
using BKConnect.Controllers;
using BKConnect.BKConnectBE.Common;
using BKConnect.Model.Dtos.User;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Responses>> GetUserById(string id)
        {
            var u = await userService.GetUserByIdAsync(id);
            if (u == null) return BadRequest(this.Error("UserNotfound"));
            Response.Cookies.Append("refresh_token", "abc", new CookieOptions
            {
                HttpOnly = true, // ??t cookie là HTTP-only
                Secure = true, // ??m b?o cookie ch? ???c g?i qua HTTPS
                SameSite = SameSiteMode.Strict, // Thi?t l?p SameSite
                MaxAge = TimeSpan.FromDays(30) // Th?i gian s?ng c?a cookie (ví d?: 30 ngày)
            });
            return this.Success(u, MsgNo.SUCCESS);
        }

        [HttpPost]
        public async Task<ActionResult<Responses>> UpdateUser(UpdateUserDto userDto)
        {
            await userService.UpdateUserAsync(userDto);
            var refresh = Request.Cookies["refresh_token"];
            return this.Success("Oke", MsgNo.SUCCESS);
        }
    }
}