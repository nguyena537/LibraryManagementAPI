using LibraryManagementAPI.Models;
using LibraryManagementAPI.Models.DTOs;

namespace LibraryManagementAPI.Services
{
    public interface ICustomerService
    {
        Task<ServiceResponse<List<BookDto>>> GetBooksCurrentlyBorrowed();
        Task<ServiceResponse<List<BookDto>>> CheckoutBook(int bookId);
        Task<ServiceResponse<List<BookDto>>> ReturnBook(int bookId);
    }
}
