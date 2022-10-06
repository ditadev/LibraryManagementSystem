using Library.Api.Dtos;
using Library.Features;
using Library.Model;
using Library.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
// [Authorize]
public class LibraryController : ControllerBase
{
    private readonly DataContext _dataContext;
    private readonly ILibraryService _libraryService;

    public LibraryController(DataContext dataContext, ILibraryService libraryService)
    {
        _dataContext = dataContext;
        _libraryService = libraryService;
    }

    [HttpGet]
    // [Authorize(Roles = "Customer")]
    public async Task<ActionResult<List<Book>>> GetBooks()
    {
        var books = await _libraryService.GetAllBooks();
        return Ok(books);
    }
    
    [HttpGet]
    // [Authorize(Roles = "Customer")]
    public async Task<ActionResult<List<Book>>> SearchBookByTitle(string title)
    {
        var books = await _libraryService.GetBookByTitle(title);
        if (books==null)
        {
            return BadRequest("No Book Found :(");
        }
        return Ok(books);
    }
    
    [HttpGet]
    // [Authorize(Roles = "Customer")]
    public async Task<ActionResult<List<Book>>> SearchBookByGenre(string title)
    {
        var books = await _libraryService.GetBookByGenre(title);
        if (books==null)
        {
            return BadRequest("No Book Found :(");
        }
        return Ok(books);
    }

}