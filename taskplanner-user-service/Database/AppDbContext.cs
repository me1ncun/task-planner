using Microsoft.EntityFrameworkCore;
using taskplanner_user_service.Models;
using Task = taskplanner_user_service.Models.Task;

namespace taskplanner_user_service.Database;

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