using taskplanner_user_service.DTOs;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Services.Interfaces;

public interface IUserService
{
    Task<RegisterUserResponse> Register(RegisterUserRequest request);
    Task<LoginUserResponse> Login(LoginUserRequest request);
    Task<UpdateUserResponse> UpdatePassword(UpdateUserRequest request);
    Task<List<GetTaskRequest>> GetAll();
}