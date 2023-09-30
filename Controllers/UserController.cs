using Microsoft.AspNetCore.Mvc;
using BKConnectBE.Repository;
using Microsoft.AspNetCore.Authorization;
using BKConnectBE.Model.Entities;
using BKConnectBE.Model.Dtos;
using BKConnect.Service;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IGenericRepository<User> _userRepository;

        private IJwtService _jwtService;

        public UserController(IGenericRepository<User> userREpository, IJwtService jwtService)
        {
            _userRepository = userREpository;
            _jwtService = jwtService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var accesstoken = _jwtService.GenerateAccessToken();
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null) return BadRequest("id not found");
            return Ok(new UserDto(u));
        }

        [HttpPost]
        public Task<IActionResult> UpdateUser()
        {
            throw new Exception("Loi update");
        }
    }
}