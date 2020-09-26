using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RideShare.API.Models;
using RideShare.Core;
using RideShare.Domain;

namespace RideShare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult Create([FromBody] User user)
        {
            var result = _userService.Create(user);
            if (result)
            {
                return Ok(new ResponseModel
                {
                    Message = "User created successfully",
                });
            }

            return BadRequest(new ResponseModel
            {
                Message = "Bad request"
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult Authenticate([FromBody] User user)
        {
            var userToAuthenticate = _userService.Authenticate(user.Phone, user.Password);

            if (userToAuthenticate == null)
                return BadRequest(new {message = "Phone or password is incorrect"});

            return Ok(userToAuthenticate);
        }
    }
}