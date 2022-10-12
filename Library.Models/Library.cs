using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Library.Model;

public class Library
{
    public string LibraryId { get; set; }
    public string LibraryName { get; set; }
    [JsonIgnore] public List<Book> Books { get; set; }
    [JsonIgnore] public List<User> Customers { get; set; }
    [JsonIgnore] public List<Administrator> Administrators { get; set; }
}