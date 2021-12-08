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

        public void Add(AddBookDto book)
        {
            var newBook = _mapper.Map<Book>(book);

            try
            {
                _libraryDatabase.Books.Add(newBook);
                _libraryDatabase.SaveChanges();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public bool CheckBookExistById(int id)
        {
            try
            {
                var getBook = _libraryDatabase.Books.SingleOrDefault(i => i.Id == id);

                if (getBook == null)
                    return false;
                return true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public List<AuthorView> GetAuthorList()
        {
            var authors = _libraryDatabase.Authors.ToList();
            var authorViewList = new List<AuthorView>();

            foreach (var author in authors)
            {
                authorViewList.Add(_mapper.Map<AuthorView>(author));
            }

            return authorViewList;
        }

        public BookDetailDto GetBookById(int id)
        {
            var getBook = _libraryDatabase.Books.Include(a => a.Author).SingleOrDefault(i => i.Id == id);
            var bookDetail = _mapper.Map<BookDetailDto>(getBook);

            return bookDetail;
        }

        public List<BookListDto> GetBookList()
        {
            var bookList = _libraryDatabase.Books.ToList();

            if (bookList == null)
                throw new ArgumentNullException();

            var bookListDto = new List<BookListDto>();
            foreach (var book in bookList)
            {
                bookListDto.Add(_mapper.Map<BookListDto>(book));
            }

            return bookListDto;
        }

        public void Delete(int bookId)
        {
            var book = _libraryDatabase.Books.SingleOrDefault(b => b.Id == bookId);

            _libraryDatabase.Books.Remove(book);
            _libraryDatabase.SaveChanges();
        }

        public bool CheckThereSameBook(string bookName, string authorName)
        {
            var bName = _libraryDatabase.Books.Include(a => a.Author).Where(b => b.Name == bookName).ToList();

            if (bName.Count == 0)
                return false;

            var aName = bName.Where(a => a.Author.Name == authorName).ToList();

            if (aName.Count == 0)
                return false;

            return true;
        }

        public BookListDto SearchByName(string bookName)
        {
            var book = _libraryDatabase.Books.SingleOrDefault(n => n.Name == bookName);

            if (book == null)
                throw new ArgumentNullException();
            var bookDto = _mapper.Map<BookListDto>(book);

            return bookDto;
        }

        public string GetAuthorNameById(int AuthorId)
        {
            var author = _libraryDatabase.Authors.SingleOrDefault(i => i.Id == AuthorId);
            return author.Name;
        }

        public bool CheckAuthorExistByName(string AuthorName)
        {
            var aName = _libraryDatabase.Authors.FirstOrDefault(n => n.Name == AuthorName);

            if (aName != null)
                return true;
            return false;
        }

        public void AddAuthor(AddAuthorDto newAuthorDto)
        {
            var newAuthor = _mapper.Map<Author>(newAuthorDto);

            try
            {
                _libraryDatabase.Authors.Add(newAuthor);
                _libraryDatabase.SaveChanges();
            }
            catch (Exception error)
            {

                throw new Exception(error.Message);
            }
        }

        void ILibraryService.Delete(int bookId)
        {
            throw new NotImplementedException();
        }

        public void DeleteAuthor(int Id)
        {
            Author author = _libraryDatabase.Authors.SingleOrDefault(a => a.Id == Id);

            _libraryDatabase.Authors.Remove(author);
            _libraryDatabase.SaveChanges();
        }

        public bool CheckAuthorExistById(int id)
        {
            Author author = _libraryDatabase.Authors.SingleOrDefault(i => i.Id == id);

            if (author == null)
                return false;

            return true;
        }

        public AuthorDetailDto GetAuthorById(int id)
        {
            Author author = _libraryDatabase.Authors.Include(b => b.Books).SingleOrDefault(i => i.Id == id);

            AuthorDetailDto authorDetailDto = _mapper.Map<AuthorDetailDto>(author);
            authorDetailDto.BookCount = author.Books.Count;

            return authorDetailDto;
        }
    }
}
