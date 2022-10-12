using System.ComponentModel.DataAnnotations;

namespace Library.Api.Dtos;

public class RegisterUserRequest
{
    public long UserId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Address { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be more than 6 characters")]
    public string Password { get; set; }

    [Required] [Compare("Password")] public string ConfirmPassword { get; set; }
}