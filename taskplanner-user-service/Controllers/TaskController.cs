using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace taskplanner_user_service.Controllers;

public class TaskController : Controller
{
    [Authorize]
    [HttpGet("/tasks")]
    public IActionResult GetTasks()
    {
        return Ok();
    }
    
}