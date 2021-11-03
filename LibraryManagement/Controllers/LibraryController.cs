using LibraryManagement.Entities;
using LibraryManagement.Service;
using LibraryManagement.Web.Models;
using LibraryManagement.Web.Models.ViewModelDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Controllers
{

    public class LibraryController : Controller
    {
        private readonly ILibraryService _iLibraryService;

        public LibraryController(ILibraryService ilibraryService)
        {
            _iLibraryService = ilibraryService;
        }

        [Route("Library/Index")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Library/AddBook")]
        [HttpGet]
        public IActionResult AddBook()
        {
            var newBookDto = new AddBookDto();
            return View(newBookDto);
        }

        [Route("Library/AddBook")]
        [HttpPost]
        public IActionResult AddBook(AddBookDto newBookDto)
        {
            if (newBookDto == null)
                throw new ArgumentNullException();

            if (!ModelState.IsValid)
                return View();

            var newAuthor = new Author()
            {
                Name = newBookDto.AuthorName,
                Age = newBookDto.AuthorAge
            };
            var newBook = new Book
            {
                Name = newBookDto.BookName,
                Price = newBookDto.BookPrice,
                Description = newBookDto.BookDescription,
                Author = newAuthor
            };

            _iLibraryService.Add(newBook);
            TempData["AddBook"] = "کتاب با موفقیت ثبت شد";

            return RedirectToAction("AddBook");
        }

        [Route("Library/BookList")]
        [HttpGet]
        public IActionResult BookList()
        {
            var bookList = _iLibraryService.GetBookList();

            if (bookList == null)
                throw new ArgumentNullException();

            var bookListDto = new List<BookListDto>();
            foreach (var book in bookList)
            {
                bookListDto.Add(new BookListDto
                {
                    Id = book.Id,
                    Book = book.Name,
                    Author = book.Author.Name,
                    Description = book.Description
                });
            }

            return View(bookListDto);
        }

        [Route("Library/DeleteBook/{Id}")]
        [HttpGet]
        public IActionResult DeleteBook(int Id)
        {

            _iLibraryService.Delete(Id);
            TempData["DeleteBook"] = "کتاب با موفقیت حذف شد";

            return RedirectToAction("BookList");
        }

    }
}
