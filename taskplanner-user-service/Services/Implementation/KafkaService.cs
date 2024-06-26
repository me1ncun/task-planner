using System.Text;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Unicode;
using Confluent.Kafka;
using RabbitMQ.Client;
using taskplanner_user_service.Contracts;
using taskplanner_user_service.Models;
using Task = System.Threading.Tasks.Task;

namespace taskplanner_user_service.Services.Implementation;

public class KafkaService
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly ILogger<KafkaService> _logger;
    private readonly IProducer<Null, string> _producer;

    public KafkaService(IOptions<KafkaSettings> kafkaSettings, ILogger<KafkaService> logger)
    {
        _kafkaSettings = kafkaSettings.Value;
        _logger = logger;
        
        var producerconfig = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
        };

        _producer = new ProducerBuilder<Null, string>(producerconfig).Build();
    }

    public void SendMessage(object obj)
    {        
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };
        var message = JsonSerializer.Serialize(obj, options);
        
        ProduceAsync(message);
    }
    
    public async Task ProduceAsync(string message)
    {
        try
        {
            var kafkamessage = new Message<Null, string> { Value = message, };

            await _producer.ProduceAsync(_kafkaSettings.Topic, kafkamessage);
            
            _logger.LogInformation("Sended message: " + message);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing Kafka message: {ex.Message}");
        }
    }
}