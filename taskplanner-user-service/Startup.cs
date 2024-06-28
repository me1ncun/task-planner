using Microsoft.Extensions.DependencyInjection.Extensions;
using taskplanner_user_service.Extensions;

namespace taskplanner_user_service;

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
        services.AddJwtCollection(Configuration)
            .AddDatabase(Configuration)
            .AddServicesAndRepositories()
            .AddSwagger()
            .AddCorsPolicy()
            .AddKafka(Configuration)
            .AddAutoMapper()
            .AddEmailService();
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        services.AddJwtAuthentication(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.ApplyMigrations();
        }
        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseCors();
        
        app.UseCookiePolicy(new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.None,
            Secure = CookieSecurePolicy.None
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}