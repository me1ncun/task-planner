using taskplanner_user_service.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Services.Interfaces;

public interface IUserService
{
    public Task<string> Login(string email, string password, string repeatedPassword);
    public Task Register(string email, string password);
    public Task UpdatePassword(string email, string newPassword, string repeatedPassword);
}