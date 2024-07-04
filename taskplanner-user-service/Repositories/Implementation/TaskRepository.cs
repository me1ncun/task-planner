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
    
    public async Task InsertAsync(Models.Task task)
    {
        await _appDbContext.Tasks.AddAsync(task);
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task<List<Models.Task>> GetByUserIdAsync(int? id)
    {
        var tasks = await _appDbContext.Tasks.Where(t => t.UserId == id).ToListAsync();
        
        return tasks;
    }
    
    public async Task<Models.Task> GetByIdAsync(int id)
    {
        var task = await _appDbContext.Tasks.FindAsync(id);
        
        return task;
    }
    
    public async Task<Models.Task> GetByTitleAsync(string title)
    {
        var task = await _appDbContext.Tasks.FirstAsync(x => x.Title == title);
        
        return task;
    }
    
    public async Task UpdateAsync(string title, string description, string status, DateTime doneAt)
    {
        var task = await GetByTitleAsync(title);
        
        task.Title = title;
        task.Description = description;
        task.Status = status;
        task.DoneAt = doneAt;
        
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(int id, string title, string description, string status, DateTime doneAt)
    {
        var task = await GetByIdAsync(id);
        
        task.Title = title;
        task.Description = description;
        task.Status = status;
        task.DoneAt = doneAt;
        
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var task = await _appDbContext.Tasks.FindAsync(id);
        
        _appDbContext.Tasks.Remove(task);
        await _appDbContext.SaveChangesAsync();
    }
}