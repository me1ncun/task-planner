using Coravel;
using Coravel.Invocable;
using Coravel.Scheduling.Schedule.Interfaces;
using taskplanner_scheduler.Helpers;
using taskplanner_scheduler.Repositories;
using taskplanner_scheduler.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScheduler();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<MailHelper>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
builder.Services.AddScoped<RabbitMqService>();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add the scheduler to the application (it will iterate every day at 00:00)
app.Services.UseScheduler(scheduler =>
{
    scheduler.Schedule<RabbitMqService>().DailyAt(00, 00).Zoned(TimeZoneInfo.Local).PreventOverlapping(nameof(RabbitMqService)); 
}).LogScheduledTaskProgress(app.Services.GetRequiredService<ILogger<IScheduler>>());

app.UseHttpsRedirection();

app.MapControllers();

app.Run();