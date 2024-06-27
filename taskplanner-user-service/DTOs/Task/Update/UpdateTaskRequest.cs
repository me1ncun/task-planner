using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.DTOs;

public record UpdateTaskRequest(
    [Required] string Title,
    [Required] string Description,
    [Required] string Status,
    DateTime DoneAt
);