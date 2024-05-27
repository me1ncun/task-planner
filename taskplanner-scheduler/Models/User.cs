using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace taskplanner_scheduler.Models;
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}