using Azure;
using BusinessLayer.Interfaces;
using Common;
using Common.DAO;
using Common.DTO;
using Common.Modals;
using DataAccessLayer.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersBL _userBL;
        private IBus bus;
        private readonly ILogger<UsersController> _logger;
        private readonly TokenHelper _tokenHelper;

        public UsersController(IUsersBL userBL, ILogger<UsersController> logger, IBus bus, TokenHelper tokenHelper)
        {
            this._userBL = userBL;
            this.bus = bus;
            this._tokenHelper = tokenHelper;
            this._logger = logger;
            _logger.LogInformation($"UserController initialized at {DateTime.Now}");
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<object>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<object>))]
        public async Task<IActionResult> Register([FromBody] RegisterDTO user)
        {
            _logger.LogInformation($"Register method called with email: {user.Email}");

            bool isUserRegistered = await _userBL.IsRegisteredAsync(user.Email);
            if (isUserRegistered)
            {
                _logger.LogWarning($"Account already exists with email: {user.Email}");
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = $"Account already exists with this email {user.Email}",
                    Data = null
                });
            }

            _logger.LogInformation($"Creating user with email: {user.Email}");
            bool isCreated = _userBL.CreateUser(user);

            if (isCreated)
            {
                _logger.LogInformation($"User created successfully with email: {user.Email}");

                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "User created successfully.",
                    Data = null
                });
            }

            _logger.LogError($"An error occurred while creating the user with email: {user.Email}");
            return StatusCode(500, new ResponseModel<object>
            {
                Success = false,
                Message = "An error occurred while creating the user.",
                Data = null
            });
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            _logger.LogInformation($"Login attempt for email: {user.Email}");

            var loginResDTO = await _userBL.Login(user);

            if (loginResDTO.Token == null)
            {
                _logger.LogWarning($"Invalid login credentials for email: {user.Email}");
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Invalid login credentials", Data = null });
            }

            _logger.LogInformation($"Login successful for email: {user.Email}");
            return Ok(new ResponseModel<LoginResDTO> { Success = true, Message = "Login successful", Data = loginResDTO });
        }

        [HttpPost]
        [Route("forgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgetPassword(string Email)
        {
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(Email, emailPattern))
            {
                _logger.LogWarning("Invalid email format");
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid email format. Examples of valid emails: 'example@gmail.com'"
                });
            }

            _logger.LogInformation($"Forget password request received for email: {Email}");
            try
            {
                bool isUserRegistered = await _userBL.IsRegisteredAsync(Email);
                if (isUserRegistered)
                {
                    Send send = new Send();
                    ForgetPasswordDTO forgetPasswordModel = _userBL.ForgetPassword(Email);
                    send.SendMail(forgetPasswordModel.Email, forgetPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost/FundooNotesEmailQueue");

                    var endPoint = await bus.GetSendEndpoint(uri);
                    await endPoint.Send(forgetPasswordModel);

                    _logger.LogInformation($"Mail sent successfully for email: {Email}");
                    return Ok(new ResponseModel<string> { Success = true, Message = "Mail sent successfully", Data = null });
                }
                else
                {
                    _logger.LogWarning($"Account doesn't exist for email: {Email}");
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Account doesn't exist with this email",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred during forget password process for email: {Email}. Error: {ex.Message}");
                throw;
            }
        }

        [Authorize(AuthenticationSchemes = "ResetPasswordScheme")]
        [HttpPost]
        [Route("resetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            _logger.LogInformation($"POST request received for password reset");

            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrWhiteSpace(token) || !_tokenHelper.ValidateResetPasswordToken(token))
            {
                _logger.LogWarning($"Invalid or expired token received.");
                return Unauthorized(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Invalid or expired token.",
                    Data = null
                });
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            bool result = await _userBL.ResetPassword(email, resetPassword);

            if (result)
            {
                _logger.LogInformation($"Password reset successful for email: {email}");
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Password change successful. Please remember to use your new password next time.",
                    Data = null
                });
            }

            _logger.LogWarning($"No account found with email: {email} for password reset.");
            return NotFound(new ResponseModel<string>
            {
                Success = false,
                Message = "No account found with that email address.",
                Data = null
            });
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<UserDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<object>))]
        public async Task<IActionResult> GetUserById()
        {
            int id = int.Parse(User.FindFirst("UserId")?.Value);
            _logger.LogInformation($"Fetching user profile for UserId: {id}");

            var user = await _userBL.GetUserByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return NotFound(new ResponseModel<object>
                {
                    Success = false,
                    Message = $"User with ID {id} not found.",
                    Data = null
                });
            }
            _logger.LogInformation($"User profile fetched successfully for UserId: {id}");
            return Ok(new ResponseModel<UserDTO> { Success = true, Message = "User profile fetched successfully", Data = user });
        }
    }

}
