using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.Contracts;

public record RegisterUserRequest(
    [Required] string Email,
    [Required] string Password
);