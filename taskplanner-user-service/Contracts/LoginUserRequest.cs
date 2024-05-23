using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.Contracts;

public record LoginUserRequest(
    [Required] string Email,
    [Required] string Password,
    [Required] string RepeatPassword
    );