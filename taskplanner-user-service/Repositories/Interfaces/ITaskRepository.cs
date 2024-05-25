namespace taskplanner_user_service.Repositories.Interfaces;

public interface ITaskRepository
{
    public Task Add(string title, string description, string status, int userId, DateTime doneAt);
    public Task<List<Models.Task>> GetByUserId(int id);
    public Task Update(int id, string title, string description, string status, DateTime doneAt);
    public Task Delete(int id);
}