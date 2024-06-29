using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Exceptions;
using taskplanner_scheduler.Helpers;
using taskplanner_scheduler.Services.Implementation;

namespace taskplanner_mailservice;

[Route("[controller]")]
[ApiController]
public class ProducerController : ControllerBase
{
    private readonly ProducerService _producerService;
    
    public ProducerController(
        ProducerService producerService)
    {
        _producerService = producerService;
    }
    
    [HttpPost("/kafka/send")]
    public async Task<IActionResult> SendMessage(string message)
    {
        try
        {
            _producerService.ProduceAsync(message);
            
            return Ok("Sended message to Kafka");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("/kafka/send-report")]
    public async Task<IActionResult> SendReport()
    {
        try
        {
            _producerService.SendDailyReport();
            
            return Ok("Sended daily report to Kafka");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}