namespace taskplanner_user_service.Repositories.Interfaces;

public interface ITaskRepository
{
    Task Add(string title, string description, string status, int userId, DateTime doneAt);
    Task<List<Models.Task>> GetByUserId(int id);
    Task Update(int id, string title, string description, string status, DateTime doneAt);
    Task Delete(int id);
}