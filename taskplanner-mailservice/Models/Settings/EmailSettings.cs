namespace taskplanner_mailservice.Models;

public class EmailSettings
{
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public string ReceiverEmail { get; set; }
    public bool EnableSSL { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}