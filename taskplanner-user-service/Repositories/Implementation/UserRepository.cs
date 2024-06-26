using Microsoft.EntityFrameworkCore;
using taskplanner_user_service.Database;
using taskplanner_user_service.Exceptions;
using taskplanner_user_service.Models;
using taskplanner_user_service.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Repositories.Implementation;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _appDbContext;
    
    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async Task Add(string email, string password)
    {
        var userExist = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userExist != null)
        {
            throw new ThrownException("A user with this email already exists");
        }
        
        var user = new User
        {
            Email = email,
            Password = password
        };
        
        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task<User> GetByEmail(string email)
    {
        var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        
        return user;
    }
    
    public async Task UpdatePassword(string email, string password)
    {
        var user = await GetByEmail(email);
        
        user.Password = password;
        
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task<List<User>> GetAll()
    {
        return await _appDbContext.Users.ToListAsync();
    }
}