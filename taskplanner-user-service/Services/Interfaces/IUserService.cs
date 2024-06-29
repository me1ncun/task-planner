using System.Security.Claims;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.DTOs.Auth;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Services.Interfaces;

public interface IUserService
{
    Task<RegisterUserResponse> Register(RegisterUserRequest request);
    Task<LoginUserResponse> Login(LoginUserRequest request);
    Task<UpdateUserResponse> UpdatePassword(UpdateUserRequest request);
    Task<List<GetUserResponse>> GetAll();
    int? GetUserIdIfAuthenticated(ClaimsPrincipal user);
}