using AutoMapper;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly LibraryDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerService(IMapper mapper, LibraryDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetMemberId() => int.Parse(_httpContextAccessor.HttpContext.User
            .FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<BookDto>>> GetBooksCurrentlyBorrowed()
        {
            var serviceResponse = new ServiceResponse<List<BookDto>>();
            var dbBooks = await _context.Books
                .Where(b => b.borrowers.Any(m => m.MemberId == GetMemberId()))
                .ToListAsync();

            serviceResponse.Data = dbBooks.Select(b => _mapper.Map<BookDto>(b)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<BookDto>>> CheckoutBook(int bookId)
        {
            ServiceResponse<List<BookDto>> response = new ServiceResponse<List<BookDto>>();

            try
            {
                var customer = await _context.Members
                    .Include(m => m.Books)
                    .FirstOrDefaultAsync(m => m.MemberId == GetMemberId());
                var book = await _context.Books
                    .FirstOrDefaultAsync(b => b.BookId == bookId);
                if (book == null)
                {
                    response.Success = false;
                    response.Message = "Book was not found.";
                }
                else if (book.borrowers.Contains(customer))
                {
                    response.Success = false;
                    response.Message = "You have already borrowed this book.";
                }
                else
                {
                    if (book.numAvailable > 0)
                    {
                        book.numAvailable--;
                        book.borrowers.Add(customer);

                        customer.Books.Add(book);

                        CheckoutDto checkout = new CheckoutDto();
                        checkout.BookId = bookId;
                        checkout.MemberId = GetMemberId();
                        checkout.CheckedOutOn = DateTime.Now;
                        _context.Checkouts.Add(_mapper.Map<Checkout>(checkout));

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "This book is not available.";
                    }

                }

                var dbBooks = await _context.Books
                    .Where(b => b.borrowers.Any(m => m.MemberId == GetMemberId()))
                    .ToListAsync();
                response.Data = dbBooks.Select(b => _mapper.Map<BookDto>(b)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }


        public async Task<ServiceResponse<List<BookDto>>> ReturnBook(int bookId)
        {
            ServiceResponse<List<BookDto>> response = new ServiceResponse<List<BookDto>>();

            try
            {
                var customer = await _context.Members
                    .Include(m => m.Books)
                    .FirstOrDefaultAsync(m => m.MemberId == GetMemberId());
                var book = customer.Books
                    .FirstOrDefault(b => b.BookId == bookId);

                if (book != null)
                {
                    book.numAvailable++;
                    book.borrowers.Remove(customer);

                    customer.Books.Remove(book);

                    var checkout = await _context.Checkouts
                        .FirstOrDefaultAsync(c => c.BookId == bookId && c.MemberId == GetMemberId());

                    await _context.SaveChangesAsync();
                }
                else
                {
                    response.Success = false;
                    response.Message = "You do not currently own this book, or this book is not in the library database.";
                }

                var dbBooks = await _context.Books
                    .Where(b => b.borrowers.Any(m => m.MemberId == GetMemberId()))
                    .ToListAsync();
                response.Data = dbBooks.Select(b => _mapper.Map<BookDto>(b)).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }


            return response;
        }
    }
}
