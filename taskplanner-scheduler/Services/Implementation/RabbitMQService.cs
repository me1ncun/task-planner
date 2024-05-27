using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Coravel.Invocable;
using Coravel.Scheduling.Schedule.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using taskplanner_scheduler.Helpers;
using taskplanner_scheduler.Models;
using taskplanner_scheduler.Repositories;
using taskplanner_scheduler.Services.Implementation;
using Task = System.Threading.Tasks.Task;
using TaskService = taskplanner_scheduler.Services.Implementation.TaskService;

public class RabbitMqService : IRabbitMqService, IInvocable
{
    private readonly RabbitMqSettings _rabbitMQSettings;
    private readonly UserService _userService;
    private readonly MailHelper _mailHelper;
    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService(IOptions<RabbitMqSettings> rabbitMQSettings,
        UserService userService,
        MailHelper mailHelper,
        ILogger<RabbitMqService> logger)
    {
        _rabbitMQSettings = rabbitMQSettings.Value;
        _userService = userService;
        _mailHelper = mailHelper;
        _logger = logger;
    }

    public Task Invoke()
    { 
        return SendDailyReports();
    }

    public void SendMessage(object obj)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        
        var message = JsonSerializer.Serialize(obj, options);
        
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQSettings.Host, 
            Port = _rabbitMQSettings.Port,
            UserName = _rabbitMQSettings.UserName,
            Password = _rabbitMQSettings.Password
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _rabbitMQSettings.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                routingKey: _rabbitMQSettings.QueueName,
                basicProperties: null,
                body: body);
            
            Console.WriteLine("Отправлено сообщение: " + message);
        }
    }
    
    protected async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            
            SendDailyReports();
        }
    }

    public async Task SendDailyReports()
    {
        var users = await _userService.GetUsers();
        
        foreach (var user in users)
        {
            var emailMessage = await CreateEmailMessage(user);
            
            SendMessage(emailMessage);
        }
    }
    
    private async Task<EmailMessage> CreateEmailMessage(User user)
    {
        return new EmailMessage
        {
            To = user.Email,
            Subject = "Ежедневный отчет с сайта TaskPlanner",
            Body = await _mailHelper.CreateMailBody(user)
        };
    }
}