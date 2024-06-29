using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.Exceptions;
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
    private readonly EmailCreatorFactory _emailCreatorFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthController> _logger;
        
    public AuthController(
        IUserService userService,
        IHttpContextAccessor  contextAccessor,
        KafkaService kafkaService,
        EmailCreatorFactory emailCreatorFactory,
        IMapper mapper,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _contextAccessor = contextAccessor;
        _kafkaService = kafkaService;
        _emailCreatorFactory = emailCreatorFactory;
        _mapper = mapper;
        _logger = logger;
    }
    
    [HttpPost("/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        try
        {
            var user = await _userService.Login(request);

            _contextAccessor.HttpContext.Response.Cookies.Append("token", user.Token, new CookieOptions
            {
                MaxAge = TimeSpan.FromMinutes(20),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(user);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (PasswordNotMatchException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    
    [HttpPost("/auth/register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
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
                
            var greetingMessage = _emailCreatorFactory.CreateEmailMessage(request);
            
            _kafkaService.SendMessage(greetingMessage);

            return Ok(user);
        }
        catch (AlreadyExistException ex)
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
        try
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
        catch (Exception ex)
        {
            _logger.LogInformation("Failed to logout user");
            
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        }
    }
}