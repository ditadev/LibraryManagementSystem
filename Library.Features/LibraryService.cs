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

    public async Task<List<Book>> GetAllBooks()
    {
        var book = await _dataContext.Books
            .Include(b => b.Library).ToListAsync();
        return book;
    }
    
    public async Task<Book> AddBook(Book book)
    {
        var library = await _dataContext.Libraries.Where(l => l.LibraryId == book.LibraryId)
            .FirstOrDefaultAsync();
        var addBook = new Book
        {
            ISBN = book.ISBN,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            Available = true,
            Library = library,
            LibraryId = book.LibraryId,
            Collected = DateTime.MinValue,
            ReturnDate = DateTime.MinValue
        };
         _dataContext.Books.Add(addBook);
         await _dataContext.SaveChangesAsync();
         return (addBook);
    }

    public async Task DeleteBook(string ISBN)
    {
        var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.ISBN == ISBN);
        _dataContext.Books.Remove(book);
        _dataContext.SaveChangesAsync();
    }

    public async Task<List<Book>> GetBookByTitle(string name)
    {
        var books = await _dataContext.Books
            .Where(b => b.Title.Contains(name)).ToListAsync();
        if (books.Count==0)
        {
            return null;
        }
        return books;
    }
    public async Task<List<Book>> GetBookByGenre(string name)
    {
        var books = await _dataContext.Books
            .Where(b => b.Title.Contains(name)).ToListAsync();
        if (books.Count==0)
        {
            return null;
        }
        return books;
    }

    public async Task<Book?> UpdateBook(Book book)
    {
        var oldBook = await _dataContext.Books.Where(b => b.ISBN == book.ISBN).SingleOrDefaultAsync();
        if (oldBook==null)
        {
            return null;
        }

        oldBook.Title = oldBook.Title;
        oldBook.ISBN = oldBook.ISBN;
        oldBook.Author = oldBook.Author;
        oldBook.Genre = oldBook.Genre;
        oldBook.LibraryId = oldBook.LibraryId;
        oldBook.Library = oldBook.Library;
        oldBook.Available = book.Available;
        oldBook.Collected = oldBook.Collected;
        oldBook.ReturnDate=DateTime.UtcNow.AddDays(7);

        _dataContext.Books.Update(oldBook);
        await _dataContext.SaveChangesAsync();
        return oldBook;
    }
}