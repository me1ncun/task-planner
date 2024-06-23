using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Implementation;

namespace taskplanner_mailservice.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEmailSenderService(this IServiceCollection services)
    {
        return services
            .AddScoped<EmailSenderService>();
    }
    
    public static IServiceCollection AddConsumerService(this IServiceCollection services)
    {
        return services
            .AddScoped<ConsumerService>();
    }
    
    public static IServiceCollection AddEmailSettings(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<EmailSettings>(configuration.GetSection("MailSettings"));
    }
    
    public static IServiceCollection AddKafkaSettings(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .Configure<KafkaSettings>(configuration.GetSection("ApacheKafka"));
    }
    
}