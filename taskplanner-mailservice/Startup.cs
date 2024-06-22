using System.Reflection;
using Microsoft.OpenApi.Models;
using taskplanner_mailservice.Extensions;

namespace taskplanner_mailservice;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        // Application services
        services.AddEmailSenderService()
            .AddRabbitMqListener()
            .AddEmailSettings(Configuration)
            .AddRabbitMqSettings(Configuration);


        services.AddControllers();
        services.AddEndpointsApiExplorer();
       
        services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Email Service API V1");
                    c.RoutePrefix = string.Empty;
                });
        }
        
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}