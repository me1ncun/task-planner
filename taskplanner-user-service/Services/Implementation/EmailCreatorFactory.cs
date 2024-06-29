using taskplanner_user_service.DTOs;

namespace taskplanner_user_service.Services.Implementation;

public class EmailCreatorFactory
{
    public EmailMessage CreateEmailMessage(RegisterUserRequest request)
    {
        return new EmailMessage
        {
            To = request.Email,
            Subject = "Registration on Task Planner",
            Body = "Successful registration! Welcome in our community!",
        };
    }
}