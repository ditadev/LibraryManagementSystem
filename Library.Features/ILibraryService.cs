using Library.Model;

namespace Library.Features;

public interface ILibraryService
{
    public Task<List<Book>> GetBooks();
}