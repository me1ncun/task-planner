using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Interfaces;

namespace taskplanner_mailservice.Services.Implementation;

public class RabbitMQReceiver: BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IEmailSenderService _emailService;

    public RabbitMQReceiver(IOptions<RabbitMQSettings> rabbitMqSettings, IEmailSenderService emailService)
    {
        var factory = new ConnectionFactory()
        {
            HostName = rabbitMqSettings.Value.Host,
            UserName = rabbitMqSettings.Value.UserName,
            Password = rabbitMqSettings.Value.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: rabbitMqSettings.Value.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        _emailService = emailService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var emailMessage = JsonSerializer.Deserialize<EmailMessage>(message);
            await _emailService.SendEmailAsync(emailMessage);
        };
        _channel.BasicConsume(queue: "EMAIL_SENDING_TASKS", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}