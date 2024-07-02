using System.Security.Claims;
using AutoMapper;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.DTOs.Auth;
using taskplanner_user_service.Exceptions;
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
    private readonly IMapper _mapper;
    
    public UserService(
        IUserRepository userRepository,
        JwtProvider jwtProvider,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
        _mapper = mapper;
    }
    public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
    {
        var userExist = await _userRepository.GetUserByEmailAsync(request.Email);
        if (userExist is not null)
        {
            throw new AlreadyExistException();
        }
        
        if(request.Password != request.ConfirmPassword)
        {
            throw new PasswordNotMatchException("Failed to login");
        }
        
        var user = _mapper.Map<User>(request);
        
        var hashedPassword = HashPasswordHelper.HashPassword(user.Password);
        user.Password = hashedPassword;
        
        await _userRepository.InsertAsync(user);
        
        var token = _jwtProvider.GenerateToken(user);

        var response = _mapper.Map<RegisterUserResponse>(user);
        response.Token = token;
        
        return response;
    }
    
    public async Task<LoginUserResponse> Login(LoginUserRequest request)
    {
        var userExist = await _userRepository.GetUserByEmailAsync(request.Email);
        if (userExist is null)
        {
            throw new EntityNotFoundException();
        }
        
        var result = HashPasswordHelper.VerifyPassword(request.Password, userExist.Password);
        if (result != true)
        {
            throw new PasswordNotMatchException("Failed to login");
        }

        var user = _mapper.Map<LoginUserResponse>(userExist);
        
        var token = _jwtProvider.GenerateToken(userExist);
        user.Token = token;
        
        return user;
    }
    
    public async Task<UpdateUserResponse> UpdatePassword(UpdateUserRequest request)
    {
        var userExist = await _userRepository.GetUserByEmailAsync(request.Email);
        if(userExist is null)
        {
            throw new EntityNotFoundException();
        }
        
        if(request.NewPassword != request.ConfirmPassword)
        {
            throw new PasswordNotMatchException();
        }
        
        var user = _mapper.Map<User>(request);
        
        var hashedPassword = HashPasswordHelper.HashPassword(request.NewPassword);
        user.Password = hashedPassword;
        
        await _userRepository.UpdatePasswordAsync(user.Email, user.Password);
        
        var response = _mapper.Map<UpdateUserResponse>(user);
        
        return response;
    }
    
    public async Task<List<GetUserResponse>> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
        if (users is null)
        {
            throw new EntityNotFoundException();
        }
        
        var usersDto = _mapper.Map<List<GetUserResponse>>(users);
        
        return usersDto;
    }
    
    public int? GetUserIdIfAuthenticated(ClaimsPrincipal user)
    {
        int? userId = Int32.Parse(user.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);
        if (userId is null)
        {
            throw new InvalidOperationException("Invalid or expired token.");
        }

        return userId;
    }
}