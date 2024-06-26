namespace taskplanner_user_service.Services.Interfaces;

public interface ITaskService
{ 
    Task Add(string title, string description, string status, int userId, DateTime doneAt);
    Task<List<Models.Task>> GetByUserId(int id);
    Task Update(string title, string description, string status, DateTime doneAt);
    Task Delete(int id);
}