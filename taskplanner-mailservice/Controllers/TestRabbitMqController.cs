using Microsoft.AspNetCore.Mvc;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Implementation;

namespace taskplanner_mailservice.Controllers;

[Route("[controller]")]
[ApiController]
public class TestRabbitMqController : ControllerBase
{
    private readonly RabbitMqListener _rabbitMqListener;
    
    public TestRabbitMqController(RabbitMqListener rabbitMqListener)
    {
        _rabbitMqListener = rabbitMqListener;
    }
    
    [HttpGet("/rabbit/listen")]
    public async Task<IActionResult> Start()
    {
        await _rabbitMqListener.StartAsync(CancellationToken.None);
        
        return Ok("Слушатель запущен");
    }
}