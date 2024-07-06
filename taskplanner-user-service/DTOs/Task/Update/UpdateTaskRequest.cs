using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.DTOs;

public class UpdateTaskRequest
{
    [Required] public int Id { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Status { get; set; }
}