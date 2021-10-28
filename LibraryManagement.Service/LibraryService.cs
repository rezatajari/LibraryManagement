using LibraryManagement.DataAccessLayer;
using LibraryManagement.Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task Add(Book newBook)
        {
            await _libraryDatabase.Books.AddAsync(newBook);
            _libraryDatabase.SaveChanges();
        }

        public List<Book> GetBookList()
        {
            var bookList = _libraryDatabase.Books.Include(a => a.Author).ToList();
            return bookList;
        }
        public async Task Delete(int bookId)
        {
            var book = _libraryDatabase.Books.SingleOrDefault(b => b.Id == bookId);

            _libraryDatabase.Books.Remove(book);
            await _libraryDatabase.SaveChangesAsync();
        }

    }
}
