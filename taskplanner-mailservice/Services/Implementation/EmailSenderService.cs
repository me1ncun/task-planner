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

    public void SendGreetingEmail()
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(_emailSettings.SenderEmail);
        mail.To.Add(new MailAddress(_emailSettings.ReceiverEmail));
        mail.Subject = "Регистрация на TaskPlanner";
        mail.Body = "Вы успешно зарегестрировались, вы можете пользоваться всем функционалом сайта";

        SmtpClient client = new SmtpClient();
        client.Host = _emailSettings.SmtpHost;
        client.Port = _emailSettings.SmtpPort;
        client.EnableSsl = _emailSettings.EnableSSL;
        client.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
        client.Send(mail);
    }

    public void SendEmail(string subject, string body)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(_emailSettings.SenderEmail);
        mail.To.Add(new MailAddress(_emailSettings.ReceiverEmail));
        mail.Subject = subject;
        mail.Body = body;

        SmtpClient client = new SmtpClient();
        client.Host = _emailSettings.SmtpHost;
        client.Port = _emailSettings.SmtpPort;
        client.EnableSsl = _emailSettings.EnableSSL;
        client.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
        client.Send(mail);
    }

}