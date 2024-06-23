using Microsoft.AspNetCore.Mvc;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Implementation;

namespace taskplanner_mailservice.Controllers;

[Route("[controller]")]
[ApiController]
public class ConsumerController : ControllerBase
{
    private readonly ConsumerService _consumerService;
    
    public ConsumerController(ConsumerService consumerService)
    {
        _consumerService = consumerService;
    }
    
    [HttpGet("/kafka/listen")]
    public async Task<IActionResult> Start()
    {
        try
        {
            await _consumerService.StartAsync(CancellationToken.None);
        
            return Ok("Слушатель запущен");
        }
        catch (Exception e)
        { 
            return BadRequest(e.Message);
        }
    }
}