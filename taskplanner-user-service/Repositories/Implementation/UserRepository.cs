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
    
    public async Task InsertAsync(User user)
    {
        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _appDbContext.Users.
            FirstOrDefaultAsync(u => u.Email == email);
        
        return user;
    }
    
    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _appDbContext.Users.
            FirstOrDefaultAsync(u => u.Id == id);
        
        return user;
    }
    
    public async Task UpdatePasswordAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        
        user.Password = password;
        
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        return await _appDbContext.Users.ToListAsync();
    }
}