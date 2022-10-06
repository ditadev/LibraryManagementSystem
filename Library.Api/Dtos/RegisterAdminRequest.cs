using System.ComponentModel.DataAnnotations;

namespace Library.Api.Dtos;

public class RegisterAdminRequest
{
    public string AdminId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be more than 6 characters")]
    public string Password { get; set; }

    [Required] [Compare("Password")] public string ConfirmPassword { get; set; }
}