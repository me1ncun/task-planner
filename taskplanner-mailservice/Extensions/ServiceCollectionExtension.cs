using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Implementation;
using taskplanner_mailservice.Services.Interfaces;

namespace taskplanner_mailservice.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEmailSenderService(this IServiceCollection services)
    {
        return services
            .AddScoped<IEmailSenderService, EmailSenderService>();
    }
    
    public static IServiceCollection AddRabbitMqListener(this IServiceCollection services)
    {
        return services
            .AddScoped<RabbitMqListener>();
    }
    
    public static IServiceCollection AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<EmailSettings>(configuration.GetSection("MailSettings"));
    }
    
    public static IServiceCollection AddRabbitMqSettings(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"));
    }
    
}