namespace taskplanner_mailservice.Services.Interfaces;

public interface IRabbitMqService
{
    void SendMessage(object obj);
    void SendMessage(string message);
}