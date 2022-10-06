using System.Text.Json.Serialization;

namespace Library.Api.Dtos;

public class BookDto
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Author { get; set; }
    public bool? Available { get; set; }
    public string CustomerId { get; set; }
    public string LibraryId { get; set; }
    [JsonIgnore]public DateTime Collected { get; set; }
    [JsonIgnore]public DateTime ReturnDate { get; set; }
}