using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Implementation;
using taskplanner_mailservice.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<RabbitMqListener>();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();