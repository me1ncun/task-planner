using Microsoft.EntityFrameworkCore;
using taskplanner_user_service.Models;

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
    public DbSet<taskplanner_user_service.Models.Task> Tasks { get; set; }
}