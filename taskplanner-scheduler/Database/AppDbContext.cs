using Microsoft.EntityFrameworkCore;
using taskplanner_scheduler.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_scheduler.Database;

public class AppDbContext : DbContext
{
    private IConfiguration Configuration;
    
    public AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("Database"));
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Task> Tasks { get; set; }
}