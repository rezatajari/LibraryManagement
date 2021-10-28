using LibraryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
