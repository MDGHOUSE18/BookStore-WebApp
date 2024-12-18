using BusinessLayer.Interfaces;
using Common;
using Common.DAO;
using Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserBL _userBL;

        public UsersController(IUserBL userBL)
        {
            this._userBL = userBL;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterDTO user)
        {
            if (user == null)
            {
                return BadRequest("User data is null");
            }

            bool isCreated = _userBL.CreateUser(user);

            if (isCreated)
            {
                return Ok("User created successfully.");
            }

            return StatusCode(500, "An error occurred while creating the user.");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            if (user == null)
            {
                return BadRequest("User data is null");
            }

            var token = await _userBL.Login(user);

            if (token == null)
            {
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid login credentials", Data = null });
            }

            return Ok(new ResponseModel<string> { Success = true, Message = "Login successful", Data = token });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserById()
        {
            int id =int.Parse(User.FindFirst("UserId")?.Value);

            var user = await _userBL.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            return Ok(new ResponseModel<UserDTO> { Success = true, Message = "User profile fetched successfully", Data = user });
        }

    }
}
