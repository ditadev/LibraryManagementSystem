using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library.Model;

public class Administrator
{
    [Required]
    public string AdminId { get; set; }
    [JsonIgnore]
    public string FirstName { get; set; }
    [JsonIgnore]
    public string LastName { get; set; }
    [JsonIgnore] public string? PasswordHash { get; set; }
    [Required]
    public string Password { get; set; }
    [JsonIgnore] public string? PasswordResetToken { get; set; }
    [JsonIgnore] public DateTime? ResetTokenExpires { get; set; }
    [JsonIgnore] public Library Library { get; set; }
}