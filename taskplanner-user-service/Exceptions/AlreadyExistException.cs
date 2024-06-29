namespace taskplanner_user_service.Exceptions;

[Serializable]
public class AlreadyExistException: Exception
{
    public AlreadyExistException(string message) : base(message)
    {
    }
    
    public AlreadyExistException() : base("Entity already exists")
    {
    }
}