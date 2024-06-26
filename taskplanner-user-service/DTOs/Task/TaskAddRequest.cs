using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.Contracts;

public record TaskAddRequest(
    [Required] string Title,
    [Required] string Description,
    [Required] string Status, 
    DateTime DoneAt
);