using LibraryManagementAPI.Models;
using LibraryManagementAPI.Models.DTOs;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    [Authorize(Roles = "Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ILibrarianService _librarianService;
        private readonly ICustomerService _customerService;

        public CustomerController(ILibrarianService librarianService, ICustomerService customerService)
        {
            _librarianService = librarianService;
            _customerService = customerService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetAllBooks()
        {
            return Ok(await _librarianService.GetAllBooks());
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetBooksByTitle(string title)
        {
            return Ok(await _librarianService.GetBooksByTitle(title));
        }

        [HttpGet("{authorLastName}")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetBooksByAuthor(string authorFirstName, string authorLastName)
        {
            return Ok(await _librarianService.GetBooksByAuthor(authorFirstName, authorLastName));
        }

        [HttpGet("GetCurrentlyBorrowed")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> GetBooksCurrentlyBorrowed()
        {
            return Ok(await _customerService.GetBooksCurrentlyBorrowed());
        }

        [HttpPut("CheckoutBook")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> CheckoutBook(int bookId)
        {
            return Ok(await _customerService.CheckoutBook(bookId));
        }

        [HttpPut("ReturnBook")]
        public async Task<ActionResult<ServiceResponse<List<BookDto>>>> ReturnBook(int bookId)
        {
            return Ok(await _customerService.ReturnBook(bookId));
        }
    }
}
