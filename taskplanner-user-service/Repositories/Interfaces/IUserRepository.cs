using taskplanner_user_service.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Repositories.Interfaces;

public interface IUserRepository
{
    Task InsertAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(int id);
    Task UpdatePasswordAsync(string email, string password);
    Task<List<User>> GetAllAsync();
}