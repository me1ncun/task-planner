using System.ComponentModel.DataAnnotations;

namespace taskplanner_user_service.DTOs;

public class RegisterUserRequest
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string ConfirmPassword { get; set; }
}
