using Microsoft.AspNetCore.Mvc;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Interfaces;

namespace taskplanner_mailservice.Controllers;

[Route("[controller]")]
[ApiController]
public class EmailController: ControllerBase
{
    private readonly IEmailSenderService _emailSenderService;
    
    public EmailController(IEmailSenderService emailSenderService)
    {
        _emailSenderService = emailSenderService;
    }
    
    [HttpPost]
    public void SendEmail(string to, string subject, string body)
    {
        _emailSenderService.SendEmail(new EmailMessage
        {
            To = to,
            Subject = subject,
            Body = body
        });
    }
    
}