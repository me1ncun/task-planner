namespace taskplanner_user_service.Exceptions;

[Serializable]
public class NotFoundException: Exception
{
    public NotFoundException(string message): base(message)
    {
    }
    
    public NotFoundException(): base("Task not found")
    {
    }
}