using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Interfaces;

namespace taskplanner_mailservice.Services.Implementation;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailSettings _emailSettings;
    public EmailSenderService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async void SendEmail(EmailMessage emailMessage)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(_emailSettings.SenderEmail);
        mail.To.Add(new MailAddress(emailMessage.To));
        mail.Subject = emailMessage.Subject;
        mail.Body = emailMessage.Body;

        SmtpClient client = new SmtpClient();
        client.Host = _emailSettings.SmtpServer;
        client.Port = _emailSettings.SmtpPort;
        client.EnableSsl = _emailSettings.EnableSsl;
        client.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password); 
        
        await client.SendMailAsync(mail);
    }

}