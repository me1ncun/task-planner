namespace taskplanner_user_service.Repositories.Interfaces;

public interface ITaskRepository
{
    Task InsertAsync(Models.Task task);
    Task<List<Models.Task>> GetByUserIdAsync(int? id);
    Task UpdateAsync(string title, string description, string status, DateTime doneAt);
    Task UpdateAsync(int id, string title, string description, string status, DateTime doneAt);
    Task DeleteAsync(int id);
    Task<Models.Task> GetByTitleAsync(string title);
    Task<Models.Task> GetByIdAsync(int id);
}