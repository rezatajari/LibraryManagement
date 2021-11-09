using AutoMapper;
using LibraryManagement.DataTransferObjects;
using LibraryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            // مپ کردن دریافت کتاب از دیتابیس به مدل لیست برای ویو
            CreateMap<Book, BookListDto>()
                .ForMember(des => des.Book, opt => opt.MapFrom(src => src.Name))
                .ForMember(des => des.Author, opt => opt.MapFrom(src => src.Author.Name));

            // مپ کردن در ویو برای حالت دریافت کتاب و ذخیره آن در مدل دیتابیس
            CreateMap<AddBookDto, Book>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.BookName))
                .ForMember(des => des.Description, opt => opt.MapFrom(src => src.BookDescription))
                .ForMember(des => des.Price, opt => opt.MapFrom(src => src.BookPrice))
                .ForPath(des => des.Author.Name, opt => opt.MapFrom(src => src.AuthorName))
                .ForPath(des => des.Author.Age, opt => opt.MapFrom(src => src.AuthorAge));
        }
    }
}
