using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace taskplanner_user_service.Models;

[Table("tasks")]
public class Task
{
    [Column("id")]
    public int Id { get; set; }
    [Column("title")]
    public string Title { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("status")]
    public string Status { get; set; }
    [Column("user_id")]
    [ForeignKey("User")]
    public int UserId { get; set; }
    [Column("done_at")]
    public DateTime? DoneAt { get; set; }
}
