﻿public interface IRabbitMqService
{
    void SendMessage(object obj);
    void SendMessage(string message);
    public Task SendDailyReports();
}