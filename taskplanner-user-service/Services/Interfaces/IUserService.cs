namespace taskplanner_user_service.Services.Interfaces;

public interface IUserService
{
    public Task<string> Login(string email, string password);
    public Task Register(string email, string password);
}