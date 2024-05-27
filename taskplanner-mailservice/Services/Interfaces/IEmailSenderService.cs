using taskplanner_mailservice.Models;

namespace taskplanner_mailservice.Services.Interfaces;

public interface IEmailSenderService
{
    public void SendEmail(EmailMessage emailMessage);
}