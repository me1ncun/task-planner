using taskplanner_user_service.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Services.Interfaces;

public interface IUserService
{
    Task<string> Login(string email, string password, string repeatedPassword);
    Task Register(string email, string password);
    Task UpdatePassword(string email, string newPassword, string repeatedPassword);
    Task<List<User>> GetAll();
}