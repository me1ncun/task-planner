namespace taskplanner_user_service.Exceptions;

[Serializable]
public class ThrownException: Exception
{
    public ThrownException(string message) : base(message)
    {
    }

    public ThrownException() : base(String.Format("An exception was thrown"))
    {
    }
}