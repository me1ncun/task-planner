using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.Contracts;

public record UpdateUserRequest(
    [Required] string Email,
    [Required] string NewPassword,
    [Required] string ConfirmPassword
);