﻿using LibraryManagement.DataTransferObjects;
using LibraryManagement.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagement.Service
{
    public interface ILibraryService
    {
        Task Add(AddBookDto book);

        Task Delete(int bookId);

        Task<List<BookListDto>> GetBookList();

        Task<bool> CheckBookExistById(int id);

        Task<BookDetailDto> GetBookById(int id);

        Task<bool> CheckThereSameBook(string bookName, string authorName);

        Task<BookListDto> SearchByName(string bookName);

        Task<List<AuthorView>> GetAuthorList();

        Task<string> GetAuthorNameById(int AuthorId);

        Task<bool> CheckAuthorExistByName(string AuthorName);

        Task AddAuthor(AddAuthorDto newAuthorDto);

        Task DeleteAuthor(int Id);

        Task<bool> CheckAuthorExistById(int id);

        Task<AuthorDetailDto> GetAuthorById(int id);
    }
}
