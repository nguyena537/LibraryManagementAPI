using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementAPI.Services
{
    public class LibrarianService : ILibrarianService
    {
        private readonly IMapper _mapper;
        private readonly LibraryDbContext _context;

        public LibrarianService(IMapper mapper, LibraryDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<BookDto>> AddBook(BookDto newBook)
        {
            var serviceResponse = new ServiceResponse<BookDto>();
            Book book = _mapper.Map<Book>(newBook);

            bool bookInList = false;
            foreach(var b in _context.Books)
            {
                if (b.AuthorFirstName == book.AuthorFirstName && b.AuthorLastName == book.AuthorLastName && b.Title == book.Title && 
                    b.ISBN == book.ISBN && b.PublicationYear == book.PublicationYear && b.Publisher == book.Publisher && b.Cost == book.Cost)
                {
                    bookInList = true;
                    book.BookId = b.BookId;
                    newBook.BookId = b.BookId;
                }
            }
            if (!bookInList)
            {
                _context.Books.Add(book);
            }
            else
            {
                var bookToUpdate = await _context.Books
                    .FirstOrDefaultAsync(b => b.BookId == book.BookId);

                book.Title = bookToUpdate.Title;
                book.AuthorFirstName = bookToUpdate.AuthorFirstName;
                book.AuthorLastName = bookToUpdate.AuthorLastName;
                book.ISBN = bookToUpdate.ISBN;
                book.PublicationYear = bookToUpdate.PublicationYear;
                book.Publisher = bookToUpdate.Publisher;
                book.Cost = bookToUpdate.Cost;
                book.numAvailable = bookToUpdate.numAvailable + book.numAvailable;
                newBook.numAvailable = book.numAvailable;
                bookToUpdate.numAvailable = book.numAvailable;
            }
            
            await _context.SaveChangesAsync();
            serviceResponse.Data = newBook;
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<BookDto>> DeleteBook(int id)
        {
            ServiceResponse<BookDto> serviceResponse = new ServiceResponse<BookDto>();

            try
            {
                var book = await _context.Books
                    .FirstOrDefaultAsync(b => b.BookId == id);
                serviceResponse.Data = _mapper.Map<BookDto>(book);

                if (book != null)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                    
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Book not found";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<BookDto>> EditBookInfo(BookDto updatedBook)
        {
            ServiceResponse<BookDto> serviceResponse = new ServiceResponse<BookDto>();

            try
            {
                var book = await _context.Books
                    .FirstOrDefaultAsync(b => b.BookId == updatedBook.BookId);

                book.Title = updatedBook.Title;
                book.AuthorFirstName = updatedBook.AuthorFirstName;
                book.AuthorLastName = updatedBook.AuthorLastName;
                book.ISBN = updatedBook.ISBN;
                book.PublicationYear = updatedBook.PublicationYear;
                book.Publisher = updatedBook.Publisher;
                book.Cost = updatedBook.Cost;
                book.numAvailable = updatedBook.numAvailable;
                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<BookDto>(book);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BookDto>>> GetAllBooks()
        {
            var serviceResponse = new ServiceResponse<List<BookDto>>();
            var dbBooks = await _context.Books
                .ToListAsync();
            serviceResponse.Data = dbBooks.Select(b => _mapper.Map<BookDto>(b)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<MemberDto>>> GetAllMembers()
        {
            var serviceResponse = new ServiceResponse<List<MemberDto>>();
            var dbBooks = await _context.Members
                .Where(m => m.Role == "Customer")
                .ToListAsync();
            serviceResponse.Data = dbBooks.Select(b => _mapper.Map<MemberDto>(b)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BookDto>>> GetBooksByAuthor(string authorFirstName, string authorLastName)
        {
            var serviceResponse = new ServiceResponse<List<BookDto>>();
            var dbBooks = await _context.Books
                .Where(b => b.AuthorFirstName == authorFirstName && b.AuthorLastName == authorLastName)
                .ToListAsync();
            serviceResponse.Data = dbBooks.Select(b => _mapper.Map<BookDto>(b)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BookDto>>> GetBooksByTitle(string title)
        {
            var serviceResponse = new ServiceResponse<List<BookDto>>();
            var dbBooks = await _context.Books
                .Where(b => b.Title == title)
                .ToListAsync();
            serviceResponse.Data = dbBooks.Select(b => _mapper.Map<BookDto>(b)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BookDto>>> GetBooksCurrentlyBorrowed(int memberId)
        {
            var serviceResponse = new ServiceResponse<List<BookDto>>();
            var dbBooks = await _context.Books
                .Where(b => b.borrowers.Any(m => m.MemberId == memberId))
                .ToListAsync();

            serviceResponse.Data = dbBooks.Select(b => _mapper.Map<BookDto>(b)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<MemberDto>>> GetMemberByLastName(string lastName)
        {
            var serviceResponse = new ServiceResponse<List<MemberDto>>();
            var dbBooks = await _context.Members
                .Where(m => m.Role == "Customer")
                .Where(m => m.LastName == lastName)
                .ToListAsync();
            serviceResponse.Data = dbBooks.Select(b => _mapper.Map<MemberDto>(b)).ToList();
            return serviceResponse;
        }
    }
}
