using LibraryManagementAPI.Models;
using LibraryManagementAPI.Models.DTOs;

namespace LibraryManagementAPI.Services
{
    public interface ILibrarianService
    {
        Task<ServiceResponse<List<BookDto>>> GetAllBooks();
        Task<ServiceResponse<List<BookDto>>> GetBooksByAuthor(string authorFirstName, string authorLastName);
        Task<ServiceResponse<List<BookDto>>> GetBooksByTitle(string title);
        Task<ServiceResponse<BookDto>> AddBook(BookDto newBook);
        Task<ServiceResponse<BookDto>> EditBookInfo(BookDto updatedBook);
        Task<ServiceResponse<BookDto>> DeleteBook(int id);   
        Task<ServiceResponse<List<MemberDto>>> GetAllMembers();
        Task<ServiceResponse<List<MemberDto>>> GetMemberByLastName(string lastName);
        Task<ServiceResponse<List<BookDto>>> GetBooksCurrentlyBorrowed(int memberId);
    }
}
