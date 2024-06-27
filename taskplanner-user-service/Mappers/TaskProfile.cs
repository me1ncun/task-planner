using AutoMapper;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.Models;
using Task = taskplanner_user_service.Models.Task;

namespace taskplanner_user_service.Mappers;

public class TaskProfile: Profile
{
    public TaskProfile()
    {
        CreateMap<AddTaskRequest, Task>();
        CreateMap<Task, AddTaskResponse>();
        CreateMap<Task, UpdateTaskResponse>();
        CreateMap<Task, DeleteTaskResponse>();
        CreateMap<Task, GetTaskResponse>();
    }
}