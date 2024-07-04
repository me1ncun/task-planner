namespace taskplanner_user_service.Helpers;

public static class DateHelper
{
    public static DateTime GetDateTimeByStatus(string status)
    {
        if (status == "Done")
        {
            return DateTime.Now.ToUniversalTime();
        }
        else
        {
            return DateTime.Parse("0001-01-01T00:00:00Z");
        }
    }
}