using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.DTOs.Auth;
using taskplanner_user_service.Exceptions;
using taskplanner_user_service.Models;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("/user")]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);
            var userEmailClaim = User.Claims.FirstOrDefault(x => x.Type == "userEmail")?.Value;

            var user = new GetUserResponse()
            {
                Id = userIdClaim,
                Email = userEmailClaim
            };

            if (user is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "Invalid or expired token." });
            }

            return Ok(user);
        }

        [HttpPost("/user/reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] UpdateUserRequest request)
        {
            try
            {
                await _userService.UpdatePassword(request);

                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (PasswordNotMatchException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("/users")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();

            return Ok(users);
        }
    }
}