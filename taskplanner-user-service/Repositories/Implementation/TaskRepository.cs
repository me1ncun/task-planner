using Microsoft.EntityFrameworkCore;
using taskplanner_user_service.Database;
using taskplanner_user_service.Models;
using taskplanner_user_service.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Repositories.Implementation;

public class TaskRepository: ITaskRepository
{
    private readonly AppDbContext _appDbContext;
    
    public TaskRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task Add(string title, string description, string status, int userId, DateTime doneAt)
    {
        var task = new Models.Task()
        {
            Title = title,
            Description = description,
            Status = status,
            UserId = userId,
            DoneAt = doneAt,
        };
        
        await _appDbContext.Tasks.AddAsync(task);
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task<List<Models.Task>> GetByUserId(int id)
    {
        var tasks = await _appDbContext.Tasks.Where(t => t.UserId == id).ToListAsync();
        
        return tasks;
    }
    
    public async Task Update(int taskId, string title, string description, string status, DateTime doneAt)
    {
        var task = await _appDbContext.Tasks.FindAsync(taskId);
        if (task == null)
        {
            throw new NullReferenceException("Task not found");
        }
        
        task.Title = title;
        task.Description = description;
        task.Status = status;
        task.DoneAt = doneAt;
        
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task Delete(int id)
    {
        var task = await _appDbContext.Tasks.FindAsync(id);
        if (task == null)
        {
            throw new NullReferenceException("Task not found");
        }
        
        _appDbContext.Tasks.Remove(task);
        await _appDbContext.SaveChangesAsync();
    }
}