using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Features;
using Library.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // [Authorize(Roles = "Admin")]
    public class AdminPanelController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public AdminPanelController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }
        [HttpPost]
        // [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            return Ok(await _libraryService.AddBook(book));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteBook(string ISBN)
        {
            await _libraryService.DeleteBook(ISBN);
            return Ok("Delete successful");
        }
    }
}
