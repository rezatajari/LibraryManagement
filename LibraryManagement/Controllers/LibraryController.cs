using AutoMapper;
using LibraryManagement.DataTransferObjects;
using LibraryManagement.Entities;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LibraryManagement.Web.Controllers
{

    public class LibraryController : Controller
    {
        private readonly ILibraryService _iLibraryService;
        private readonly IMapper _mapper;

        public LibraryController(ILibraryService ilibraryService, IMapper mapper)
        {
            _iLibraryService = ilibraryService;
            _mapper = mapper;
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

            var newBook = _mapper.Map<Book>(newBookDto);

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
                bookListDto.Add(_mapper.Map<BookListDto>(book));
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
