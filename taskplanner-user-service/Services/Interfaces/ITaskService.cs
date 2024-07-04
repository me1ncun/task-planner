using taskplanner_user_service.DTOs;

namespace taskplanner_user_service.Services.Interfaces;

public interface ITaskService
{
    Task<AddTaskResponse> Add(AddTaskRequest request);
    Task<List<GetTaskResponse>> GetTasksByUserId(GetTaskRequest request);
    Task<UpdateTaskResponse> Update(UpdateTaskRequest request);
    Task<PutTaskResponse> Update(PutTaskRequest request);
    Task<DeleteTaskResponse> Delete(DeleteTaskRequest request);
}