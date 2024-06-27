using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.DTOs;

public class AddTaskRequest{
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Status  { get; set; }
    [Required] public int UserId  { get; set; }
    public DateTime DoneAt  { get; set; }
}