using taskplanner_user_service.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Repositories.Interfaces;

public interface IUserRepository
{
    public Task Add(string email, string password);
    public Task<User> GetByEmail(string email);
}