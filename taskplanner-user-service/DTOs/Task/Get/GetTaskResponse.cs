namespace taskplanner_user_service.DTOs;

public class GetTaskResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public DateTime DoneAt { get; set; }
}