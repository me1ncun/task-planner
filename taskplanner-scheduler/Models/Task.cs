using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
namespace taskplanner_scheduler.Models;
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public int UserId { get; set; }
    public DateTime DoneAt { get; set; }
}
