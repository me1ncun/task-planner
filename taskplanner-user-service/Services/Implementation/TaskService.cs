using taskplanner_user_service.Repositories.Interfaces;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Services.Implementation;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    
    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    
    public async Task Add(string title, string description, string status, int userId, DateTime doneAt)
    {
        await _taskRepository.Add(title, description, status, userId, doneAt);
    }
    
    public async Task<List<Models.Task>> GetByUserId(int id)
    {
        return await _taskRepository.GetByUserId(id);
    }
    
    public async Task Update(string title, string description, string status, DateTime doneAt)
    {
        await _taskRepository.Update(title, description, status, doneAt);
    }
    
    public async Task Delete(int id)
    {
        await _taskRepository.Delete(id);
    }
}