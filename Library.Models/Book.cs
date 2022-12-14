using System.Text.Json.Serialization;

namespace Library.Model;

public class Book
{
    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Author { get; set; }
    public bool? Available { get; set; }
    [JsonIgnore]public DateTime Collected { get; set; }
    [JsonIgnore]public DateTime ReturnDate { get; set; }
    [JsonIgnore]public Library? Library { get; set; }
    public string LibraryId { get; set; }

  
}