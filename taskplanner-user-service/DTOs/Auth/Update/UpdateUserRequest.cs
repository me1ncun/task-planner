using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.DTOs;

public record UpdateUserRequest(
    [Required] string Email,
    [Required] string ForgottenPassword,
    [Required] string NewPassword,
    [Required] string ConfirmPassword
);