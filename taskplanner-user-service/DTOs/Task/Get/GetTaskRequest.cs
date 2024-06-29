using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.DTOs;

public record GetTaskRequest(
    [Required] int? Id 
    );