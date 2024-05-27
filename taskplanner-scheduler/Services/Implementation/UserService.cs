using taskplanner_scheduler.Repositories;
namespace taskplanner_scheduler.Services.Implementation;

public class UserService
{
    private readonly UserRepository _userRepository;
    
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<IEnumerable<taskplanner_scheduler.Models.User>> GetUsers()
    {
        return await _userRepository.GetAll();
    }
}