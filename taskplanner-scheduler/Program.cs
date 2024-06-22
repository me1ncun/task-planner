using System.Reflection;
using Coravel;
using Coravel.Invocable;
using Coravel.Scheduling.Schedule.Interfaces;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using taskplanner_scheduler.Database;
using taskplanner_scheduler.Filters;
using taskplanner_scheduler.Helpers;
using taskplanner_scheduler.Repositories;
using taskplanner_scheduler.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.DocumentFilter<CustomSwaggerFilter>();
    c.ResolveConflictingActions(_ => _.First());
});

builder.Services.AddScheduler();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<MailHelper>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddScoped<RabbitMqService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.Configure<taskplanner_scheduler.Models.RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sheduler Service API V1");
            c.RoutePrefix = string.Empty;
        });
}

// Add the scheduler to the application (it will iterate every day at 00:00)

app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<RabbitMqService>().DailyAt(22, 07).Zoned(TimeZoneInfo.Local).PreventOverlapping(nameof(RabbitMqService));
}).LogScheduledTaskProgress(app.Services.GetRequiredService<ILogger<IScheduler>>());


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
