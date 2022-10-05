using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Library.Model;

public class Library
{
    public string LibraryId { get; set; }
    public string LibraryName { get; set; }
    [JsonIgnore]public List<Book> Books { get; set; }
    [JsonIgnore]public List<Customer> Customers { get; set; }
    [JsonIgnore][ForeignKey("Customer")]public string Username { get; set; }
}