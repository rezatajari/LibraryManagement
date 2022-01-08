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

        public async Task<MessageContract> Add(AddBookDto book)
        {

            MessageContract response = null;
            try
            {
                var newBook = _mapper.Map<Book>(book);
                await _libraryDatabase.Books.AddAsync(newBook);
                await _libraryDatabase.SaveChangesAsync();

                response = new MessageContract
                {
                    IsSuccess = true,
                    Message = "کتاب با موفقیت ثبت گردید"
                };

                return response;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task<MessageContract<bool>> CheckBookExistById(int id)
        {

            MessageContract<bool> response = null;
            try
            {
                Book getBook = await _libraryDatabase.Books.SingleOrDefaultAsync(i => i.Id == id);

                if (getBook == null)
                {
                    response = new MessageContract<bool>()
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "همچین کتابی موجود نمی باشد"
                    };
                }
                else
                {
                    response = new MessageContract<bool>()
                    {
                        IsSuccess = true,
                        Data = true,
                    };
                }

                return response;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task<MessageContract<List<AuthorView>>> GetAuthorList()
        {

            List<AuthorView> data = new List<AuthorView>();
            MessageContract<List<AuthorView>> response = null;

            try
            {
                var authors = await _libraryDatabase.Authors.ToListAsync();

                if (authors.Count == 0)
                {
                    response = new MessageContract<List<AuthorView>>();
                    response.IsSuccess = false;
                    response.Errors.Add("نویسنده ای وجود ندارد");
                }
                else
                {
                    foreach (var author in authors)
                    {
                        data.Add(_mapper.Map<AuthorView>(author));
                    }

                    response = new MessageContract<List<AuthorView>>();
                    response.IsSuccess = true;
                    response.Data = data;
                }

                return response;
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }

        }

        public async Task<MessageContract<BookDetailDto>> GetBookById(int id)
        {

            BookDetailDto data = new BookDetailDto();
            MessageContract<BookDetailDto> response = null;
            try
            {
                Book getBook = await _libraryDatabase.Books.Include(a => a.Author).SingleOrDefaultAsync(i => i.Id == id);

                if (getBook != null)
                {
                    data = _mapper.Map<BookDetailDto>(getBook);
                    response = new MessageContract<BookDetailDto>()
                    {
                        IsSuccess = true,
                        Data = data
                    };
                }
                else
                {
                    response = new MessageContract<BookDetailDto>()
                    {
                        IsSuccess = false,
                        Message = "چنین کتابی موجود نمی باشد"
                    };
                }

                return response;
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }


        }

        public async Task<MessageContract<List<BookListDto>>> GetBookList()
        {
            List<BookListDto> data = new List<BookListDto>();
            MessageContract<List<BookListDto>> response = null;

            try
            {
                List<Book> bookList = await _libraryDatabase.Books.ToListAsync();

                if (bookList != null)
                {
                    foreach (var book in bookList)
                    {
                        data.Add(_mapper.Map<BookListDto>(book));
                    }

                    response = new MessageContract<List<BookListDto>>()
                    {
                        IsSuccess = true,
                        Data = data
                    };
                }
                else
                {
                    response = new MessageContract<List<BookListDto>>()
                    {
                        IsSuccess = false,
                        Message = "لیست کتابی موجود نمی باشد"
                    };
                }

                return response;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<MessageContract> Delete(int bookId)
        {
            MessageContract response = null;
            try
            {
                Book book = await _libraryDatabase.Books.SingleOrDefaultAsync(b => b.Id == bookId);
                if (book != null)
                {
                    _libraryDatabase.Books.Remove(book);
                    await _libraryDatabase.SaveChangesAsync();
                    response = new MessageContract()
                    {
                        IsSuccess = true,
                        Message = "کتاب با موفقیت حذف گردید"
                    };
                }
                else
                {
                    response = new MessageContract()
                    {
                        IsSuccess = false,
                        Message = "چنین کتابی در کتابخانه موجود نمی باشد"
                    };
                }

                return response;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

        }

        public async Task<MessageContract<bool>> CheckThereSameBook(string bookName, string authorName)
        {
            MessageContract<bool> response = null;

            try
            {
                var bName = await _libraryDatabase.Books.Include(a => a.Author).Where(b => b.Name == bookName).ToListAsync();
                var aName = bName.Where(a => a.Author.Name == authorName).ToList();

                if (bName.Count != 0 && aName.Count != 0)
                {
                    response = new MessageContract<bool>()
                    {
                        IsSuccess = false,
                        Data = true,
                        Message = "چنین کتابی قبلا ثبت شده است"
                    };
                }
                else
                {
                    response = new MessageContract<bool>()
                    {
                        IsSuccess = true,
                        Data = false
                    };
                }

                return response;
            }
            catch (Exception er)
            {

                throw new Exception(er.Message);
            }


        }

        public async Task<MessageContract<BookListDto>> SearchByName(string bookName)
        {
            BookListDto data = new BookListDto();
            MessageContract<BookListDto> response = null;

            try
            {
                Book book = await _libraryDatabase.Books.SingleOrDefaultAsync(n => n.Name == bookName);
                if (book != null)
                {
                    data = _mapper.Map<BookListDto>(book);
                    response = new MessageContract<BookListDto>()
                    {
                        IsSuccess = true,
                        Data = data,
                        Message = "کتاب شما پیدا شد"
                    };
                }
                else
                {
                    response = new MessageContract<BookListDto>()
                    {
                        IsSuccess = false,
                        Message = "همچین کتابی موجود نمی باشد"
                    };
                }

                return response;
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
        }

        public async Task<MessageContract<string>> GetAuthorNameById(int AuthorId)
        {
            MessageContract<string> response = null;
            try
            {
                Author author = await _libraryDatabase.Authors.SingleOrDefaultAsync(i => i.Id == AuthorId);
                if (author != null)
                {
                    response = new MessageContract<string>
                    {
                        IsSuccess = true,
                        Data = author.Name,
                    };
                }
                else
                {
                    response = new MessageContract<string>
                    {
                        IsSuccess = false,
                        Message = "همچین نام نویسنده ای نداریم"
                    };
                }

                return response;
            }
            catch (Exception)
            {
                throw new ArgumentNullException("نداریم");
            }
        }

        public async Task<MessageContract<bool>> CheckAuthorExistByName(string AuthorName)
        {
            MessageContract<bool> response = null;
            try
            {
                Author author = await _libraryDatabase.Authors.FirstOrDefaultAsync(n => n.Name == AuthorName);

                if (author != null)
                {
                    response = new MessageContract<bool>()
                    {
                        IsSuccess = true,
                        Data = true
                    };
                }
                else
                {
                    response = new MessageContract<bool>()
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "همچین نویسنده ای با این نام موجود نمی باشد"
                    };
                }

                return response;
            }
            catch (Exception err)
            {

                throw new Exception(err.Message);
            }
        }

        public async Task<MessageContract> AddAuthor(AddAuthorDto newAuthorDto)
        {
            MessageContract response = null;

            try
            {
                var newAuthor = _mapper.Map<Author>(newAuthorDto);
                await _libraryDatabase.Authors.AddAsync(newAuthor);
                await _libraryDatabase.SaveChangesAsync();

                response = new MessageContract()
                {
                    IsSuccess = true,
                    Message = "نویسنده با موفقیت ثبت گردید"
                };

                return response;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task<MessageContract> DeleteAuthor(int Id)
        {
            MessageContract response = null;

            try
            {
                Author author = await _libraryDatabase.Authors.SingleOrDefaultAsync(a => a.Id == Id);
                _libraryDatabase.Authors.Remove(author);
                await _libraryDatabase.SaveChangesAsync();
                response = new MessageContract()
                {
                    IsSuccess = true,
                    Message = "نویسنده به درستی حذف گردید"
                };
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return response;
        }

        public async Task<MessageContract<bool>> CheckAuthorExistById(int id)
        {
            MessageContract<bool> response = null;

            try
            {
                Author author = await _libraryDatabase.Authors.SingleOrDefaultAsync(i => i.Id == id);

                if (author != null)
                {
                    response = new MessageContract<bool>()
                    {
                        IsSuccess = true,
                        Data = true
                    };
                }
                else
                {
                    response = new MessageContract<bool>()
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "همچین نویسنده ای وجود ندارد"
                    };
                }

                return response;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }

        public async Task<MessageContract<AuthorDetailDto>> GetAuthorById(int id)
        {
            AuthorDetailDto data = new AuthorDetailDto();
            MessageContract<AuthorDetailDto> response = null;

            try
            {
                Author author = await _libraryDatabase.Authors.Include(b => b.Books).SingleOrDefaultAsync(i => i.Id == id);
                if (author != null)
                {
                    data = _mapper.Map<AuthorDetailDto>(author);
                    data.BookCount = author.Books.Count;

                    response = new MessageContract<AuthorDetailDto>()
                    {
                        IsSuccess = true,
                        Data = data
                    };
                }
                else
                {
                    response = new MessageContract<AuthorDetailDto>()
                    {
                        IsSuccess = false,
                        Message = "چنین نویسنده ای موجود نمی باشد"
                    };
                }

                return response;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}
