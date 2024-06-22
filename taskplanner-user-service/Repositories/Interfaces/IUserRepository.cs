using taskplanner_user_service.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Repositories.Interfaces;

public interface IUserRepository
{
    Task Add(string email, string password);
    Task<User> GetByEmail(string email);
    Task UpdatePassword(string email, string password);
    Task<List<User>> GetAll();
}