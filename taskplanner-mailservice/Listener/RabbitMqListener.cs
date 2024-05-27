using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Interfaces;

namespace taskplanner_mailservice.Services.Implementation;

public class RabbitMqListener : BackgroundService
{
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly IEmailSenderService _emailSenderService;
    
    public RabbitMqListener(IOptions<RabbitMqSettings> rabbitMQSettings, IEmailSenderService emailSenderService)
    {
        _rabbitMqSettings = rabbitMQSettings.Value;
        _emailSenderService = emailSenderService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.Host, 
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _rabbitMqSettings.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                var emailMessage = JsonSerializer.Deserialize<EmailMessage>(message);
                
                /*_emailSenderService.SendEmail(emailMessage);*/
                
                Console.WriteLine($"Получено сообщение: {message}");
            };

            channel.BasicConsume(_rabbitMqSettings.QueueName, autoAck: true, consumer: consumer);
        }

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMqSettings.Host, 
            Port = _rabbitMqSettings.Port,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.Close();
            connection.Close();
            base.Dispose();
        }
    }
}