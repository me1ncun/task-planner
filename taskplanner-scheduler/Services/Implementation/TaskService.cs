using System.Text;
using taskplanner_scheduler.Repositories;
using taskplanner_scheduler.Models;
using taskplanner_user_service.Models;

namespace taskplanner_scheduler.Services.Implementation;

public class TaskService
{
    private readonly TaskRepository _taskRepository;
    
    public TaskService(TaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    
    public async Task<IEnumerable<taskplanner_scheduler.Models.Task>> GetUsersTask(taskplanner_scheduler.Models.User user)
    {
        try
        {
            return await _taskRepository.GetUsersTask(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}