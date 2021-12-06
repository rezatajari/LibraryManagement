using LibraryManagement.DataTransferObjects;
using LibraryManagement.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagement.Service
{
    public interface ILibraryService
    {
        void Add(AddBookDto book);

        Task Delete(int bookId);

        List<BookListDto> GetBookList();

        bool CheckBookExistById(int id);

        BookDetailDto GetBookById(int id);

        bool CheckThereSameBook(string bookName, string authorName);

        BookListDto SearchByName(string bookName);

        List<AuthorView> GetAuthorList();

        string GetAuthorNameById(int AuthorId);

        bool CheckAuthorExistByName(string AuthorName);

        void AddAuthor(AddAuthorDto newAuthorDto);
    }
}
