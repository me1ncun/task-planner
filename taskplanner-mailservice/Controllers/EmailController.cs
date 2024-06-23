using Microsoft.AspNetCore.Mvc;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Implementation;

namespace taskplanner_mailservice.Controllers;

[Route("[controller]")]
[ApiController]
public class EmailController: ControllerBase
{
    private readonly EmailSenderService _emailSenderService;
    
    public EmailController(EmailSenderService emailSenderService)
    {
        _emailSenderService = emailSenderService;
    }
    
    [HttpPost("/email/send")]
    public void SendEmail(string to, string subject, string body)
    {
        var message = new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = body
        };
        
        _emailSenderService.SendEmail(message);
    }
    
}