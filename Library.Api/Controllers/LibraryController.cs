using Library.Features;
using Library.Model;
using Library.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<List<Book>>> GetBooks()
    {
        var books = await _libraryService.GetBooks();
        return Ok(books);
    }
}