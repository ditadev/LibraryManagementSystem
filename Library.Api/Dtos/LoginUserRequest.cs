using System.ComponentModel.DataAnnotations;

namespace Library.Api.Dtos;

public class LoginUserRequest
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Password { get; set; }
}