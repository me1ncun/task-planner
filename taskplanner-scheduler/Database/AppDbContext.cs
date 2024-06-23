using Microsoft.EntityFrameworkCore;
using taskplanner_scheduler.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_scheduler.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
}