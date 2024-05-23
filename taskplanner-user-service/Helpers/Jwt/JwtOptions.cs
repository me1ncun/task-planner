namespace taskplanner_user_service.Helpers;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public int ExpiresHours { get; set; }
}