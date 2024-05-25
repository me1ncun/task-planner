﻿using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using taskplanner_user_service.Contracts;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Controllers;

public class TaskController : Controller
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
    public async Task<IActionResult> CreateTask(TaskRequest request)
    {
        var userId = GetUserIdIfAuthenticated();

        await _taskService.Add(request.Title, request.Description, request.Status, userId, request.DoneAt);

        return Ok();
    }

    [Authorize]
    [HttpPut("/tasks")]
    public async Task<IActionResult> UpdateTask(TaskUpdateRequest request)
    {
        try
        {
            await _taskService.Update(request.Id, request.Title, request.Description, request.Status, request.DoneAt);

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