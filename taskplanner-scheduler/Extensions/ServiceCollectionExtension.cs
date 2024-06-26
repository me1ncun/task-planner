using Microsoft.EntityFrameworkCore;
using taskplanner_scheduler.Database;
using taskplanner_scheduler.Helpers;
using taskplanner_scheduler.Models;
using taskplanner_scheduler.Repositories;
using taskplanner_scheduler.Services.Implementation;

namespace taskplanner_sheduler.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddProducerService(this IServiceCollection services)
    {
        return services
            .AddScoped<ProducerService>();
    }
    
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"));
            });
    }
    
    public static IServiceCollection AddKafkaSettings(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<taskplanner_scheduler.Models.KafkaSettings>(configuration.GetSection("ApacheKafka"));
    }

    public static IServiceCollection AddMailHelper(this IServiceCollection services)
    {
        return services
            .AddScoped<MailHelper>();
    }
    
    public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<UserRepository>()
            .AddScoped<TaskRepository>()
            .AddScoped<UserService>()
            .AddScoped<TaskService>();
    }
    
}