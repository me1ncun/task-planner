using Microsoft.AspNetCore.Mvc;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Implementation;

namespace taskplanner_mailservice.Controllers;

[Route("[controller]")]
[ApiController]
public class RabbitMqController : ControllerBase
{
    private readonly RabbitMqListener _rabbitMqListener;
    
    public RabbitMqController(RabbitMqListener rabbitMqListener)
    {
        _rabbitMqListener = rabbitMqListener;
    }
    
    [HttpGet]
    public async Task<IActionResult> Start()
    {
        await _rabbitMqListener.StartAsync(CancellationToken.None);
        
        return Ok("Слушатель запущен");
    }
}