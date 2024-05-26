using System.Text;
using Microsoft.Extensions.Options;
using System.Text.Json;
using RabbitMQ.Client;
using taskplanner_user_service.Contracts;
using taskplanner_user_service.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Services.Implementation;

public class RabbitMqService
{
    private readonly RabbitMqSettings _rabbitMQSettings;

    public RabbitMqService(IOptions<RabbitMqSettings> rabbitMQSettings)
    {
        _rabbitMQSettings = rabbitMQSettings.Value;
    }

    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        
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
    
    public EmailMessage CreateEmailMessage(RegisterUserRequest request)
    {
        return new EmailMessage
        {
            To = request.Email,
            Subject = "Registration on Task Planner",
            Body = "Successful registration! Welcome in our community!",
        };
    }
}