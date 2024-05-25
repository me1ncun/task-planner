using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Crypto.Generators;
using taskplanner_mailservice.Models;
using taskplanner_mailservice.Services.Interfaces;

namespace taskplanner_mailservice.Services.Implementation;

public class EmailSenderService: IEmailSenderService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    private async Task SendEmailAsync(EmailMessage emailMessage)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Smtp:From"]));
        email.To.Add(MailboxAddress.Parse(emailMessage.To));
        email.Subject = emailMessage.Subject;
        email.Body = new TextPart("plain")
        {
            Text = emailMessage.Body
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}