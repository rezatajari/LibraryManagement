using LibraryManagement.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagement.Service
{
    public interface ILibraryService
    {
        Task Add(Book newBook);

        Task Delete(int bookId);

        List<Book> GetBookList();
    }
}
