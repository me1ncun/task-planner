using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Interfaces;

namespace taskplanner_mailservice.Services.Implementation;

public class RabbitMqService : IRabbitMqService
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
        // Не забудьте вынести значения "localhost" и "MyQueue"
        // в файл конфигурации
        var factory = new ConnectionFactory() { HostName = _rabbitMQSettings.Host, Port = _rabbitMQSettings.Port };
        factory.UserName = _rabbitMQSettings.UserName;
        factory.Password = _rabbitMQSettings.Password;
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
        }
    }
}