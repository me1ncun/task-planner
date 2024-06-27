namespace taskplanner_user_service.Exceptions;

[Serializable]
public class PasswordNotMatchException: Exception
{
    public PasswordNotMatchException(string message) : base(message)
    {
    }
    
    public PasswordNotMatchException() : base(String.Format("Passwords do not match"))
    {
    }
}