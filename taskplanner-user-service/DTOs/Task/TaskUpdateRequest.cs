using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.Contracts;

public record TaskUpdateRequest(
    [Required] string Title,
    [Required] string Description,
    [Required] string Status,
    DateTime DoneAt
);