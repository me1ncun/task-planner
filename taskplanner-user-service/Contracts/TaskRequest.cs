using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.Contracts;

public class TaskRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public DateTime DoneAt { get; set; }
}