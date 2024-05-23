using taskplanner_user_service.Helpers;
using taskplanner_user_service.Repositories.Interfaces;
using taskplanner_user_service.Services.Interfaces;

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
    
    public async Task<string> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmail(email);
        
        var result = HashPasswordHelper.VerifyPassword(password, user.Password);
        
        if(result == false)
        {
            throw new Exception("Failed to login");
        }
        
        var token = _jwtProvider.GenerateToken(user);
        
        return token;
    }
}