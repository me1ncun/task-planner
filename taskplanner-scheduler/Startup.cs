using Coravel;
using taskplanner_scheduler.Filters;
using taskplanner_sheduler.Extensions;

namespace taskplanner_scheduler;

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
        services.AddDatabase(Configuration)
            .AddKafkaSettings(Configuration)
            .AddProducerService()
            .AddMailHelper()
            .AddServicesAndRepositories();


        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddScheduler();

        services.AddSwaggerGen(c =>
        {
            c.DocumentFilter<CustomSwaggerFilter>();
            c.ResolveConflictingActions(_ => _.First());
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scheduler Service API V1");
                    c.RoutePrefix = string.Empty;
                });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

/*app.Services.UseScheduler(scheduler =>
{
scheduler.Schedule<ProducerService>().DailyAt(11, 02).Zoned(TimeZoneInfo.Local).PreventOverlapping(nameof(ProducerService));
}).LogScheduledTaskProgress(app.Services.GetRequiredService<ILogger<IScheduler>>());*/

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}