using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.Contracts;
using taskplanner_user_service.DTO;
using taskplanner_user_service.Models;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor  _context;
        
        public UserController(IUserService userService, IHttpContextAccessor  context)
        {
            _userService = userService;
            _context = context;
        }
        
        [HttpPost("/user")]
        public async Task<IActionResult> Register(RegisterUserRequest request)
        {
            try
            {
                await _userService.Register(request.Email, request.Password);

                var token = await _userService.Login(request.Email, request.Password, request.Password);
                _context.HttpContext.Response.Cookies.Append("token", token);

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
        
        [Authorize]
        [HttpGet("/user")]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
            var userEmailClaim = User.Claims.FirstOrDefault(x => x.Type == "userEmail")?.Value;

            var user = new UserDTO
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
    }
}
