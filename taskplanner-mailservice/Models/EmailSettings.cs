namespace taskplanner_mailservice.Models;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; }
    public bool EnableSsl { get; set; }
    public string Password { get; set; }
}