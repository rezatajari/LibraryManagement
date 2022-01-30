using LibraryManagement.DataTransferObjects;
using LibraryManagement.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagement.Service
{
    public interface ILibraryService
    {
        Task<MessageContract> Add(AddBookDto book);

        Task<MessageContract> Delete(int bookId);

        Task<MessageContract<List<BookListDto>>> GetBookList(int pgNum);

        Task<MessageContract<bool>> CheckBookExistById(int id);

        Task<MessageContract<BookDetailDto>> GetBookById(int id);

        Task<MessageContract<bool>> CheckThereSameBook(string bookName, string authorName);

        Task<MessageContract<List<BookListDto>>> SearchByName(string bookName);

        Task<MessageContract<List<AuthorView>>> GetAuthorList(int pgNum);

        Task<MessageContract<string>> GetAuthorNameById(int AuthorId);

        Task<MessageContract<bool>> CheckAuthorExistByName(string AuthorName);

        Task<MessageContract> AddAuthor(AddAuthorDto newAuthorDto);

        Task<MessageContract> DeleteAuthor(int Id);

        Task<MessageContract<bool>> CheckAuthorExistById(int id);

        Task<MessageContract<AuthorDetailDto>> GetAuthorById(int id);
    }
}
