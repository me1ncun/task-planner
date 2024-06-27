using AutoMapper;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.Exceptions;
using taskplanner_user_service.Repositories.Interfaces;
using taskplanner_user_service.Services.Interfaces;

namespace taskplanner_user_service.Services.Implementation;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    
    public TaskService(ITaskRepository taskRepository, IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }
    
    public async Task<AddTaskResponse> Add(AddTaskRequest request)
    {
        var taskExist = _taskRepository.GetByTitleAsync(request.Title);
        if (taskExist is not null)
        {
            throw new AlreadyExistException();
        }

        var task = _mapper.Map<Models.Task>(request);
        
        await _taskRepository.InsertAsync(task);
        
        var response = _mapper.Map<AddTaskResponse>(task);
        return response;
    }
    
    public async Task<List<GetTaskResponse>> GetTasksByUserId(GetTaskRequest request)
    {
        var tasks = _taskRepository.GetByUserIdAsync(request.Id);
        
        var tasksDto = _mapper.Map<List<GetTaskResponse>>(tasks);
        return tasksDto;
    }
    
    public async Task<UpdateTaskResponse> Update(UpdateTaskRequest request)
    {
        var task = _taskRepository.GetByTitleAsync(request.Title);
        if (task is null)
        {
            throw new EntityNotFoundException();
        }
        
        await _taskRepository.UpdateAsync(request.Title, request.Description, request.Status, request.DoneAt);
        
        var response = _mapper.Map<UpdateTaskResponse>(task);
        return response;
    }
    
    public async Task<DeleteTaskResponse> Delete(DeleteTaskRequest request)
    {
        var task = _taskRepository.GetByIdAsync(request.Id);
        if (task is null)
        {
            throw new EntityNotFoundException();
        }
        
        await _taskRepository.DeleteAsync(task.Id);
        
        var response = _mapper.Map<DeleteTaskResponse>(task);
        return response;
    }
}