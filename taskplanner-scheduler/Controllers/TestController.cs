using Microsoft.AspNetCore.Mvc;
using taskplanner_scheduler.Helpers;

namespace taskplanner_mailservice;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IRabbitMqService _mqService;
    
    private readonly MailHelper _mailHelper;
    
    public TestController(IRabbitMqService mqService, MailHelper mailHelper)
    {
        _mqService = mqService;
        _mailHelper = mailHelper;
    }
    
    [HttpPost]
    public async Task<IActionResult> SendMessage()
    {
        _mqService.SendDailyReports();

        return Ok("Пользователи получат отчеты по почте");
    }
}