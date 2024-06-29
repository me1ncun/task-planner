using System.Text;
using taskplanner_scheduler.Repositories;
using taskplanner_scheduler.Models;

namespace taskplanner_scheduler.Services.Implementation;

public class TaskService
{
    private readonly TaskRepository _taskRepository;
    
    public TaskService(TaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    
    public async Task<IEnumerable<Models.Task>> GetUsersTask(User user)
    {
        try
        {
            return await _taskRepository.GetUsersTasks(user);
        }
        catch (Exception e)
        {
            throw new Exception("An error occurred while fetching the user's tasks", e);
        }
    }
}