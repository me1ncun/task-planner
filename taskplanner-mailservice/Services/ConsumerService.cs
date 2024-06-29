using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using taskplanner_mailservice.Models;

namespace taskplanner_mailservice.Services.Implementation;

public class ConsumerService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly KafkaSettings _kafkaSettings;
    private readonly EmailSenderService _emailSenderService;
    private readonly ILogger<ConsumerService> _logger;

    public ConsumerService(
        IOptions<KafkaSettings> kafkaSettings,
        EmailSenderService emailSenderService,
        ILogger<ConsumerService> logger)
    {
        _kafkaSettings = kafkaSettings.Value;
        _emailSenderService = emailSenderService;
        _logger = logger;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = _kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_kafkaSettings.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            ProcessKafkaMessage(stoppingToken);

            Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }

        _consumer.Close();
    }

    public void ProcessKafkaMessage(CancellationToken stoppingToken)
    {
        try
        {
            var consumeResult = _consumer.Consume(stoppingToken);

            var message = consumeResult.Message.Value;
            
            var emailMessage = JsonSerializer.Deserialize<EmailMessage>(message);
        
            _emailSenderService.SendEmail(emailMessage);

            _logger.LogInformation($"Received inventory update: {message}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing Kafka message: {ex.Message}");
        }
    }
}