using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.Contracts;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [Authorize]
    [HttpGet("/tasks")]
    public async Task<IActionResult> GetTasks()
    {
        var userId = GetUserIdIfAuthenticated();
        
        var allTasks = await _taskService.GetByUserId(userId);
        
        return Ok(allTasks);

    }

    [Authorize]
    [HttpPost("/tasks")]
    public async Task<IActionResult> CreateTask(TaskAddRequest addRequest)
    {
        var userId = GetUserIdIfAuthenticated();

        await _taskService.Add(addRequest.Title, addRequest.Description, addRequest.Status, userId, addRequest.DoneAt);

        return Ok();
    }

    [Authorize]
    [HttpPut("/tasks")]
    public async Task<IActionResult> UpdateTask(TaskUpdateRequest request)
    {
        try
        {
            await _taskService.Update(request.Title, request.Description, request.Status, request.DoneAt);

            return Ok();
        }
        catch (NullReferenceException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("/tasks")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        try
        {
            await _taskService.Delete(id);
            return Ok();
        }
        catch (NullReferenceException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private int GetUserIdIfAuthenticated()
    {
        var userId = Int32.Parse(User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);

        if (userId == null)
        {
            throw new InvalidOperationException("Invalid or expired token.");
        }
        
        return userId;
    }
}