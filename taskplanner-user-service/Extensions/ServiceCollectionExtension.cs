using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using taskplanner_user_service.Database;
using taskplanner_user_service.Helpers;
using taskplanner_user_service.Repositories.Implementation;
using taskplanner_user_service.Repositories.Interfaces;
using taskplanner_user_service.Services.Implementation;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddJwtCollection(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)))
            .AddScoped<JwtProvider>();
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen();
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"));
            });
    }
    
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.WithOrigins("http://localhost", "http://localhost:5112")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
        });
    }

    public static IServiceCollection AddKafka(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddScoped<KafkaService>()
            .Configure<KafkaSettings>(configuration.GetSection("ApacheKafka"));
    }
    
    public static IServiceCollection AddServicesAndRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITaskService, TaskService>()
            .AddScoped<ITaskRepository, TaskRepository>();
    }
}