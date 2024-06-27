using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.DTOs;
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
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
            var userEmailClaim = User.Claims.FirstOrDefault(x => x.Type == "userEmail")?.Value;

            var user = new User
            {
                Id = Int32.Parse(userIdClaim),
                Email = userEmailClaim
            };
            
            if (user == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Invalid or expired token." });
            }
            
            return Ok(user);
        }
        
        [HttpPost("/user/reset-password")]
        public async Task<IActionResult> ResetPassword(UpdateUserRequest request)
        {
            try
            {
                await _userService.UpdatePassword(request);

                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
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
