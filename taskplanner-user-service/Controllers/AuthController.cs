using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.Contracts;
using taskplanner_user_service.Services.Implementation;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController: Controller
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor  _context;
        
    public AuthController(IUserService userService, IHttpContextAccessor  context)
    {
        _userService = userService;
        _context = context;
    }
    
    [HttpPost("/auth/login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        try
        {
            var token = await _userService.Login(request.Email, request.Password, request.RepeatPassword);
            _context.HttpContext.Response.Cookies.Append("token", token,  new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20),
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None 
            });
            
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = ex.Message });
        }
    }
    
    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        _context.HttpContext.Response.Cookies.Delete("token", new CookieOptions
        {
            MaxAge = TimeSpan.FromSeconds(1),
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.None 
        });
        
        return Ok();
    }
}