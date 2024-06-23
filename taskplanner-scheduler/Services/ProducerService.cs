using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using taskplanner_scheduler.Helpers;
using taskplanner_scheduler.Models;
using taskplanner_scheduler.Services.Implementation;
using Task = System.Threading.Tasks.Task;

public class ProducerService
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly UserService _userService;
    private readonly MailHelper _mailHelper;
    private readonly ILogger<ProducerService> _logger;
    private readonly IProducer<Null, string> _producer;

    public ProducerService(
        IOptions<KafkaSettings> kafkaSettings,
        UserService userService,
        MailHelper mailHelper,
        ILogger<ProducerService> logger)
    {
        _kafkaSettings = kafkaSettings.Value;
        _userService = userService;
        _mailHelper = mailHelper;
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