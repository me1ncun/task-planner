using System.Text;
using taskplanner_scheduler.Models;
using taskplanner_scheduler.Services.Implementation;

namespace taskplanner_scheduler.Helpers;

public class MailFactory
{
    private readonly TaskService _taskService;
    
    public MailFactory(TaskService taskService)
    {
        _taskService = taskService;
    }
    
    public async Task<string> CreateMailBody(User user)
    {
        var tasks = await _taskService.GetUsersTask(user);

        var todayDate = DateTime.Now.Date;
        
        var doneTodayTasks = tasks.Where(x => x.Status == "done" && x.DoneAt == todayDate).ToList();
        var notDoneTasks = tasks.Where(x => x.Status == "notdone").ToList();
        
        var sb = new StringBuilder();
        
        sb.Append($"<h1>Добрый день, {user.Email}!</h1>");
        
        if (notDoneTasks.Count > 0 && doneTodayTasks.Count == 0)
        {
            sb.Append($"<h2>У вас осталось {tasks.Count()} несделанных задач:</h2>");
            sb.Append("<ul>");
            
            foreach (var task in notDoneTasks)
            {
                sb.Append($"<li>{task.Title}</li>");
            }
            
            sb.Append("</ul>");
        }
        
        if(doneTodayTasks.Count > 0)
        {
            sb.Append($"<h2>За сегодня вы выполнили {doneTodayTasks.Count()} задач:</h2>");
            sb.Append("<ul>");
            
            foreach (var task in doneTodayTasks)
            {
                sb.Append($"<li>{task.Title} - {task.DoneAt}</li>");
            }
            
            sb.Append("</ul>");
        }
        
        else if(doneTodayTasks.Count > 0 && notDoneTasks.Count > 0)
        {
            sb.Append($"<h2>За сегодня вы выполнили {doneTodayTasks.Count()} задач:</h2>");
            sb.Append("<ul>");
            
            foreach (var task in doneTodayTasks)
            {
                sb.Append($"<li>{task.Title} - {task.DoneAt}</li>");
            }
            
            sb.Append("</ul>");
            
            sb.Append($"<h2>У вас осталось {notDoneTasks.Count()} несделанных задач:</h2>");
            sb.Append("<ul>");
            
            foreach (var task in notDoneTasks)
            {
                sb.Append($"<li>{task.Title}</li>");
            }
            
            sb.Append("</ul>");
        }
        
        return sb.ToString();
    }
    
    public async Task<EmailMessage> CreateEmailMessage(User user)
    {
        var message = new EmailMessage()
        {
            To = user.Email,
            Subject = "Ежедневный отчет с сайта TaskPlanner",
            Body = await CreateMailBody(user)
        };

        return message;
    }
}