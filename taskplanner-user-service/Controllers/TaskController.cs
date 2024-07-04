using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.Exceptions;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IUserService _userService;

    public TaskController(
        ITaskService taskService,
        IUserService userService)
    {
        _taskService = taskService;
        _userService = userService;
    }

    [Authorize]
    [HttpGet("/tasks")]
    public async Task<IActionResult> GetTasks()
    {
        var request = new GetTaskRequest(_userService.GetUserIdIfAuthenticated(User));

        var allTasks = await _taskService.GetTasksByUserId(request);

        return Ok(allTasks);
    }

    [Authorize]
    [HttpPost("/task")]
    public async Task<IActionResult> CreateTask([FromBody] AddTaskRequest request)
    {
        request.UserId = _userService.GetUserIdIfAuthenticated(User);

        await _taskService.Add(request);

        return Ok();
    }

    [Authorize]
    [HttpPut("/task")]
    public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskRequest request)
    {
        try
        {
            await _taskService.Update(request);

            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [Authorize]
    [HttpPut("/task/{id}")]
    public async Task<IActionResult> UpdateTask([FromRoute] int id, [FromBody] PutTaskRequest request)
    {
        try
        {
            request.Id = id;
            await _taskService.Update(request);

            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    

    [Authorize]
    [HttpDelete("/task/{id}")]
    public async Task<IActionResult> DeleteTask([FromRoute] int id)
    {
        try
        {
            var userId = _userService.GetUserIdIfAuthenticated(User);

            var request = new DeleteTaskRequest(id, userId);

            await _taskService.Delete(request);

            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedException ex)
        {
            return StatusCode(Convert.ToInt32(HttpStatusCode.Forbidden), new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}