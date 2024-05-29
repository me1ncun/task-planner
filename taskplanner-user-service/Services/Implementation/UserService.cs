using System.Security.Claims;
using taskplanner_user_service.Helpers;
using taskplanner_user_service.Models;
using taskplanner_user_service.Repositories.Interfaces;
using taskplanner_user_service.Services.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Services.Implementation;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly JwtProvider _jwtProvider;
    
    public UserService(IUserRepository userRepository, JwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }
    public async Task Register(string email, string password)
    {
        var hashedPassword = HashPasswordHelper.HashPassword(password);
        
        await _userRepository.Add(email, hashedPassword);
    }
    
    public async Task<string> Login(string email, string password, string repeatedPassword)
    {
        if(password != repeatedPassword)
        {
            throw new InvalidOperationException("Passwords do not match");
        }
        
        var user = await _userRepository.GetByEmail(email);
        
        var result = HashPasswordHelper.VerifyPassword(password, user.Password);
        
        if(result == false)
        {
            throw new InvalidOperationException("Failed to login");
        }
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
    
    public async Task UpdatePassword(string email, string newPassword, string repeatedPassword)
    {
        if(newPassword != repeatedPassword)
        {
            throw new InvalidOperationException("Passwords do not match");
        }
        
        var hashedPassword = HashPasswordHelper.HashPassword(newPassword);
        
        await _userRepository.UpdatePassword(email, hashedPassword);
    }
}