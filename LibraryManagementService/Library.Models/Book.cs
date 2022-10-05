using System.Text.Json.Serialization;

namespace Library.Model;

public class Book
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Author { get; set; }
    public bool? Available { get; set; }
    public DateTime Collected { get; set; }
    public DateTime ReturnDate { get; set; }
    public Customer Customers { get; set; }
    public string CustomerId { get; set; }
    [JsonIgnore]
    public Library Library { get; set; }
    public string LibraryName { get; set; }
}