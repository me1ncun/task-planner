using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.Contracts;
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
        
        [HttpPost("register")]
        public async Task<IResult> Register(RegisterUserRequest request)
        {
           await _userService.Register(request.Email, request.Password);
           
           return Results.Ok();
        }
        
        /*[Authorize]*/
        [HttpPost("login")]
        public async Task<IResult> Login(LoginUserRequest request)
        {
            var token = await _userService.Login(request.Email, request.Password);
            
            _context.HttpContext.Response.Cookies.Append("token", token);
            
            return Results.Ok(token);
        }
    }
}
