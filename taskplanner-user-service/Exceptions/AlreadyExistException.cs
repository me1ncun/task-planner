namespace taskplanner_user_service.Exceptions;

[Serializable]
public class AlreadyExistException: Exception
{
    public AlreadyExistException(string message) : base(message)
    {
    }
    
    public AlreadyExistException() : base(String.Format("Entity already exists"))
    {
    }
}