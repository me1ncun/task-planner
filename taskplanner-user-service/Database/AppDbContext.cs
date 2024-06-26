using Microsoft.EntityFrameworkCore;
using taskplanner_user_service.Models;
using Task = taskplanner_user_service.Models.Task;

namespace taskplanner_user_service.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
}