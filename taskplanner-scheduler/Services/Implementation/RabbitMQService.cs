using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Coravel.Invocable;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using taskplanner_scheduler.Helpers;
using taskplanner_scheduler.Models;
using taskplanner_scheduler.Services.Implementation;
using Task = System.Threading.Tasks.Task;

public class RabbitMqService : IRabbitMqService, IInvocable
{
    private readonly taskplanner_scheduler.Models.RabbitMqSettings _rabbitMQSettings;
    private readonly UserService _userService;
    private readonly MailHelper _mailHelper;
    private readonly ILogger<RabbitMqService> _logger;

    public RabbitMqService(IOptions<taskplanner_scheduler.Models.RabbitMqSettings> rabbitMQSettings,
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
        try
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
        catch (BrokerUnreachableException ex)
        {
            throw new Exception(ex.Message);
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
        var message = new EmailMessage()
        {
            To = user.Email,
            Subject = "Ежедневный отчет с сайта TaskPlanner",
            Body = await _mailHelper.CreateMailBody(user)
        };

        return message;
    }
}