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
            try
            {
                var newBook = _mapper.Map<Book>(book);
                await _libraryDatabase.Books.AddAsync(newBook);
                await _libraryDatabase.SaveChangesAsync();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task<bool> CheckBookExistById(int id)
        {
            try
            {
                var getBook = await _libraryDatabase.Books.SingleOrDefaultAsync(i => i.Id == id);

                if (getBook == null)
                    return false;
                return true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task<List<AuthorView>> GetAuthorList()
        {
            var authors = await _libraryDatabase.Authors.ToListAsync();
            var authorViewList = new List<AuthorView>();

            foreach (var author in authors)
            {
                authorViewList.Add(_mapper.Map<AuthorView>(author));
            }

            return authorViewList;
        }

        public async Task<BookDetailDto> GetBookById(int id)
        {
            var getBook = await _libraryDatabase.Books.Include(a => a.Author).SingleOrDefaultAsync(i => i.Id == id);
            var bookDetail = _mapper.Map<BookDetailDto>(getBook);

            return bookDetail;
        }

        public async Task<List<BookListDto>> GetBookList()
        {
            var bookList = await _libraryDatabase.Books.ToListAsync();

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
            Book book = await _libraryDatabase.Books.SingleOrDefaultAsync(b => b.Id == bookId);

            _libraryDatabase.Books.Remove(book);
            await _libraryDatabase.SaveChangesAsync();
        }

        public async Task<bool> CheckThereSameBook(string bookName, string authorName)
        {
            var bName = await _libraryDatabase.Books.Include(a => a.Author).Where(b => b.Name == bookName).ToListAsync();

            if (bName.Count == 0)
                return false;

            var aName = bName.Where(a => a.Author.Name == authorName).ToList();

            if (aName.Count == 0)
                return false;

            return true;
        }

        public async Task<BookListDto> SearchByName(string bookName)
        {
            var book = await _libraryDatabase.Books.SingleOrDefaultAsync(n => n.Name == bookName);

            if (book == null)
                throw new ArgumentNullException();
            var bookDto = _mapper.Map<BookListDto>(book);

            return bookDto;
        }

        public async Task<string> GetAuthorNameById(int AuthorId)
        {
            try
            {
                var author = await _libraryDatabase.Authors.SingleOrDefaultAsync(i => i.Id == AuthorId);

                return author.Name;
            }
            catch (Exception)
            {
                throw new ArgumentNullException("همچین نام نویسنده ای نداریم");
            }
        }

        public async Task<bool> CheckAuthorExistByName(string AuthorName)
        {
            var aName = await _libraryDatabase.Authors.FirstOrDefaultAsync(n => n.Name == AuthorName);

            if (aName != null)
                return true;
            return false;
        }

        public async Task AddAuthor(AddAuthorDto newAuthorDto)
        {
            var newAuthor = _mapper.Map<Author>(newAuthorDto);

            try
            {
                await _libraryDatabase.Authors.AddAsync(newAuthor);
                await _libraryDatabase.SaveChangesAsync();
            }
            catch (Exception error)
            {

                throw new Exception(error.Message);
            }
        }

        public async Task DeleteAuthor(int Id)
        {
            Author author = await _libraryDatabase.Authors.SingleOrDefaultAsync(a => a.Id == Id);

            _libraryDatabase.Authors.Remove(author);
            await _libraryDatabase.SaveChangesAsync();
        }

        public async Task<bool> CheckAuthorExistById(int id)
        {
            Author author = await _libraryDatabase.Authors.SingleOrDefaultAsync(i => i.Id == id);

            if (author == null)
                return false;

            return true;
        }

        public async Task<AuthorDetailDto> GetAuthorById(int id)
        {
            Author author = await _libraryDatabase.Authors.Include(b => b.Books).SingleOrDefaultAsync(i => i.Id == id);

            AuthorDetailDto authorDetailDto = _mapper.Map<AuthorDetailDto>(author);
            authorDetailDto.BookCount = author.Books.Count;

            return authorDetailDto;
        }
    }
}
