using LibraryManagement.DataAccessLayer;
using LibraryManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Service
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryDatabase _libraryDatabase;
       
        public LibraryService(LibraryDatabase libraryDatabase)
        {
            _libraryDatabase = libraryDatabase;
        }

        public async Task AddBook(Book newBook)
        {
            await _libraryDatabase.Books.AddAsync(newBook);
            _libraryDatabase.SaveChanges();
        }

        public async Task DeleteBook(int bookId)
        {
            var book = _libraryDatabase.Books.SingleOrDefault(b => b.Id == bookId);

            _libraryDatabase.Books.Remove(book);
            await _libraryDatabase.SaveChangesAsync();
        }
    }
}
