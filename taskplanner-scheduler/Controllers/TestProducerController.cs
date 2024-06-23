using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Exceptions;
using taskplanner_scheduler.Helpers;

namespace taskplanner_mailservice;

[Route("[controller]")]
[ApiController]
public class TestProducerController : ControllerBase
{
    private readonly ProducerService _producerServiceService;
    
    public TestProducerController(ProducerService producerServiceService)
    {
        _producerServiceService = producerServiceService;
    }
    
    [HttpPost("/kafka/send")]
    public async Task<IActionResult> SendMessage(string message)
    {
        try
        {
            _producerServiceService.ProduceAsync(message);
            
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
            _producerServiceService.SendDailyReports();
            
            return Ok("Sended message to Kafka");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}