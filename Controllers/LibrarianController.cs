using LibraryManagementAPI.Models;
using LibraryManagementAPI.Models.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Authorize(Roles = "Librarian")]
    [ApiController]
    [Route("api/[controller]")]
    public class LibrarianController : ControllerBase
    {
        private readonly ILibrarianService _LibrarianService;

        public LibrarianController(ILibrarianService LibrarianService)
        {
            _LibrarianService = LibrarianService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<BookDto>>> AddBook(BookDto newBook)
        {
            return Ok(await _LibrarianService.AddBook(newBook));
        }

        [HttpGet("GetAllBooks")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetAllBooks()
        {
            return Ok(await _LibrarianService.GetAllBooks());
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetBooksByTitle(string title)
        {
            return Ok(await _LibrarianService.GetBooksByTitle(title));
        }

        [HttpGet("{authorLastName}")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetBooksByAuthor(string authorFirstName, string authorLastName)
        {
            return Ok(await _LibrarianService.GetBooksByAuthor(authorFirstName, authorLastName));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<BookDto>>> EditBookInfo(BookDto updatedBook)
        {
            var response = await _LibrarianService.EditBookInfo(updatedBook);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<BookDto>>> DeleteBook(int id)
        {
            var response = await _LibrarianService.DeleteBook(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllMembers")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetAllMembers()
        {
            return Ok(await _LibrarianService.GetAllMembers());
        }

        [HttpGet("memberLastName/{memberLastName}")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetMemberByLastName(string memberLastName)
        {
            return Ok(await _LibrarianService.GetMemberByLastName(memberLastName));
        }

        [HttpGet("GetCurrentlyBorrowed/{memberId}")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetBooksCurrentlyBorrowed(int memberId)
        {
            return Ok(await _LibrarianService.GetBooksCurrentlyBorrowed(memberId));
        }
    }

}