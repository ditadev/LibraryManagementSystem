using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library.Model;

public class Customer
{
    public string CustomerId { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Address { get; set; }
   [JsonIgnore] public Library? Library { get; set; }
   
   [JsonIgnore]public List<Book> Books { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [JsonIgnore]public string Password { get; set; }

    [JsonIgnore] public string? PasswordHash { get; set; }

    [JsonIgnore] public string? VerificationToken { get; set; }

    [JsonIgnore] public DateTime? VerifiedAt { get; set; }

    [JsonIgnore] public string? PasswordResetToken { get; set; }

    [JsonIgnore] public DateTime? ResetTokenExpires { get; set; }
}