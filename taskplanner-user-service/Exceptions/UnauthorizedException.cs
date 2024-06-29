namespace taskplanner_user_service.Exceptions;

[Serializable]
public class UnauthorizedException: Exception
{
    public UnauthorizedException(string message) : base(message) { }
    public UnauthorizedException() : base("You are not authorized") { }
}