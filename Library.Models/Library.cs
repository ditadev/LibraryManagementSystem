namespace Library.Model;

public class Library
{
    public string LibraryName { get; set; }
    public List<Book> Books { get; set; }
    public List<Customer> Customers { get; set; }
}