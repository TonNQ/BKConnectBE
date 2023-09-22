using ChatFriend.Model.Dtos;
using ChatFriend.Model.Entities;
using ChatFriend.Repository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IGenericRepository<User> _userRepository;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IGenericRepository<User> userREpository)
        {
            _userRepository = userREpository;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var u = _userRepository.GetById(id);
            if (u == null) return BadRequest("id not found");
            return Ok(new UserDto(u));
        }

        [HttpPost]
        public IActionResult CreateUser(Account a)
        {
            _userRepository.Add(new User()
            {
                Name= "user",
                BirthDay = DateTime.Now.AddYears(-18),
            });
            _userRepository.Save();
            return Ok();
        }
    }
}