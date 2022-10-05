using System.ComponentModel.DataAnnotations;

namespace Library.Api.Dtos;

public class ResetPasswordRequest
{
    public string Token { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be more than 6 characters")]
    public string Password { get; set; }

    [Required] [Compare("Password")] public string ConfirmPassword { get; set; }
}