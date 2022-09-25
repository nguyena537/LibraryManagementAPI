using AutoMapper;
using LibraryManagementAPI.Models;
using LibraryManagementAPI.Models.DTOs;

namespace LibraryManagementAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BookDto, Book>();
            CreateMap<Book, BookDto>();
            CreateMap<Checkout, CheckoutDto>();
            CreateMap<CheckoutDto, Checkout>();
            CreateMap<Member, MemberDto>();
            CreateMap<MemberDto, Member>();
        }
    }
}
