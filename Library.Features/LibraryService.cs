using Library.Model;
using Library.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Library.Features;

public class LibraryService : ILibraryService
{
    private readonly DataContext _dataContext;

    public LibraryService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<Book>> GetBooks()
    {
        return await _dataContext.Books.ToListAsync();
    }
}