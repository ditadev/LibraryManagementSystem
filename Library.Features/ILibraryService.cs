using Library.Model;

namespace Library.Features;

public interface ILibraryService
{
    public Task<List<Book>> GetAllBooks();
    public Task<Book> AddBook(Book book);
    public Task DeleteBook(string ISBN);
    public Task<List<Book>> GetBookByTitle(string name);
    public Task<List<Book>> GetBookByGenre(string name);
    public Task<Book?> UpdateBook(Book book);
}