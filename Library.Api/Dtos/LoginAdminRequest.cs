using Microsoft.Build.Framework;

namespace Library.Api.Dtos;

public class LoginAdminRequest
{
    [Required] public string AdminId { get; set; }

    [Required] public string Password { get; set; }
}