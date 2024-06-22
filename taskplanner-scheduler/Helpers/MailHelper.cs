using System.Text;
using taskplanner_scheduler.Models;
using taskplanner_scheduler.Services.Implementation;

namespace taskplanner_scheduler.Helpers;

public class MailHelper
{
    private readonly TaskService _taskService;
    
    public MailHelper(TaskService taskService)
    {
        _taskService = taskService;
    }
    
    public async Task<string> CreateMailBody(User user)
    {
        var tasks = await _taskService.GetUsersTask(user);

        var doneTasksList = tasks.Where(t => t.Status == "Done").ToList();
        var notDoneTasksList = tasks.Where(t => t.Status == "Not done").ToList();

        string doneTasks = string.Join(", ", doneTasksList.Select(t => t.Title));
        string notDoneTasks = string.Join(", ", notDoneTasksList.Select(t => t.Title));

        string result;
        if(notDoneTasksList.Count() > 0 && doneTasksList.Count() == 0)
        {
            result = $"У вас осталось {notDoneTasksList.Count()} несделанных задач. Список: {notDoneTasks}";
        }
        else if(doneTasksList.Count() >= 1 && notDoneTasksList.Count() == 0)
        {
            result = $"За сегодня вы выполнили {doneTasksList.Count()} задач. Список: {doneTasks}";
        }
        else if(doneTasksList.Count() >= 1 && notDoneTasksList.Count() >= 1)
        {
            result = $"За сутки вы выполнили {doneTasksList.Count()} задач, список: {doneTasks}." +
                     $"У вас осталось  {notDoneTasksList.Count()} несделанных задач, список: {notDoneTasks}.";
        }
        else
        {
            result = "Не забывайте заходить на наш сайт и добавлять задачи!";
        }

        return result;
    }
}