using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using taskplanner_scheduler.Database;
using taskplanner_scheduler.Models;

namespace taskplanner_scheduler.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }
}