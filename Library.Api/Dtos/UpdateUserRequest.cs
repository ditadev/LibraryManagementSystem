using System.ComponentModel.DataAnnotations;

namespace Library.Api.Dtos;

public class UpdateUserRequest
{
    public string CustomerId { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Address { get; set; }
}