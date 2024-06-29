namespace taskplanner_user_service.Exceptions;

[Serializable]
public class EntityNotFoundException: Exception
{
    public EntityNotFoundException(string message) : base(message)
    {
    }
    
    public EntityNotFoundException() : base("Entity not found")
    {
    }
}