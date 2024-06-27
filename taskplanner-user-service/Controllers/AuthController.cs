using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.Services.Implementation;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Controllers;

[ApiController]
[Route("/service/[controller]")]
public class AuthController: ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly KafkaService _kafkaService;
    private readonly EmailService _emailService;
    private readonly IMapper _mapper;
        
    public AuthController(
        IUserService userService,
        IHttpContextAccessor  contextAccessor,
        KafkaService kafkaService,
        EmailService emailService,
        IMapper mapper)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
        _kafkaService = kafkaService;
        _emailService = emailService;
        _mapper = mapper;
    }
    
    [HttpPost("/auth/login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        try
        {
            var user = await _userService.Login(request);
            
            _contextAccessor.HttpContext.Response.Cookies.Append("token", user.Token,  new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20),
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None 
            });
            
            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            return StatusCode(StatusCodes.Status401Unauthorized, new { message = ex.Message });
        }
    }
    
    [HttpPost("/auth/register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        try
        {
            await _userService.Register(request);
            
            var user = await _userService.Login(_mapper.Map<LoginUserRequest>(request));
                    
            _contextAccessor.HttpContext.Response.Cookies.Append("token", user.Token,  new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20),
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None 
            });
                
            var greetingMessage = _emailService.CreateEmailMessage(request);
            
            _kafkaService.SendMessage(greetingMessage);

            return Ok(user);
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
    
    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        _contextAccessor.HttpContext.Response.Cookies.Delete("token", new CookieOptions
        {
            MaxAge = TimeSpan.FromSeconds(1),
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.None
        });
        
        return Ok();
    }
}