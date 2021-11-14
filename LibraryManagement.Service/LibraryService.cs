using AutoMapper;
using LibraryManagement.DataAccessLayer;
using LibraryManagement.DataTransferObjects;
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
        private readonly IMapper _mapper;

        public LibraryService(LibraryDatabase libraryDatabase, IMapper mapper)
        {
            _libraryDatabase = libraryDatabase;
            _mapper = mapper;
        }

        public async Task Add(AddBookDto book)
        {
            var newBook = _mapper.Map<Book>(book);

            await _libraryDatabase.Books.AddAsync(newBook);
            _libraryDatabase.SaveChanges();
        }

        public List<BookListDto> GetBookList()
        {
            var bookList = _libraryDatabase.Books.Include(a => a.Author).ToList();

            if (bookList == null)
                throw new ArgumentNullException();

            var bookListDto = new List<BookListDto>();
            foreach (var book in bookList)
            {
                bookListDto.Add(_mapper.Map<BookListDto>(book));
            }

            return bookListDto;
        }

        public async Task Delete(int bookId)
        {
            var book = _libraryDatabase.Books.SingleOrDefault(b => b.Id == bookId);

            _libraryDatabase.Books.Remove(book);
            await _libraryDatabase.SaveChangesAsync();
        }

        public bool CheckThereSameBook(string bookName, string authorName)
        {
            var bName = _libraryDatabase.Books.Include(a => a.Author).Where(b => b.Name == bookName).ToList();

            if (bName == null)
                return false;

            var aName = bName.Where(a => a.Author.Name == authorName).ToList();

            if (aName == null)
                return false;

            return true;
        }
    }
}
