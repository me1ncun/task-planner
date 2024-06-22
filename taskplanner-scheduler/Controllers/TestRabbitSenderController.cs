using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Exceptions;
using taskplanner_scheduler.Helpers;

namespace taskplanner_mailservice;

[Route("[controller]")]
[ApiController]
public class TestRabbitSenderController : ControllerBase
{
    private readonly IRabbitMqService _mqService;
    public TestRabbitSenderController(IRabbitMqService mqService)
    {
        _mqService = mqService;
    }
    
    [HttpPost("/rabbit/send")]
    public async Task<IActionResult> SendMessage()
    {
        try
        {
            _mqService.SendDailyReports();
            
            return Ok("Пользователи получат отчеты по почте");
        }
        catch (BrokerUnreachableException e)
        {
            return BadRequest(e.Message);
        }
    }
}