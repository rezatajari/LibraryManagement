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
            ViewBag.AuthorList = _iLibraryService.GetAuthorList();
            return View(newBookDto);
        }

        /// <summary>
        /// اضافه کردن کتاب به کتابخانه
        /// </summary>
        /// <param name="newBookDto"></param>
        /// <returns></returns>
        [Route("Library/AddBook")]
        [HttpPost]
        public IActionResult AddBook(AddBookDto newBookDto)
        {
            if (!ModelState.IsValid)
                return View();

            if (newBookDto == null)
                throw new ArgumentNullException();

            string authorName = _iLibraryService.GetAuthorNameById(newBookDto.AuthorId);

            bool checkExistBook = _iLibraryService.CheckThereSameBook(newBookDto.BookName, authorName);

            if (checkExistBook == true)
            {
                TempData["ExistBook"] = "این کتاب قبلا ثبت شده است";
                return RedirectToAction("AddBook");
            }

            _iLibraryService.Add(newBookDto);
            TempData["AddBook"] = "کتاب با موفقیت ثبت شد";

            return RedirectToAction("AddBook");
        }

        [Route("Library/AddAuthor")]
        [HttpGet]
        public IActionResult AddAuthor()
        {
            var newAuthor = new AddAuthorDto();
            return View(newAuthor);
        }

        [Route("Library/AddAuthor")]
        [HttpPost]
        public IActionResult AddAuthor(AddAuthorDto newAuthorDto)
        {

            if (!ModelState.IsValid)
                return View();

            bool checkExistAuthorName = _iLibraryService.CheckAuthorExistByName(newAuthorDto.Name);

            if (checkExistAuthorName == true)
            {
                TempData["ExistAuthor"] = "این نویسنده قبلا ثبت شده است";
                return RedirectToAction("AddAuthor");
            }

            _iLibraryService.AddAuthor(newAuthorDto);
            TempData["AddAuthor"] = "نویسنده با موفقیت ثبت شد";

            return RedirectToAction("AddAuthor");

            return View();
        }

        /// <summary>
        /// لیست کلیه کتاب های داخل کتابخانه 
        /// </summary>
        /// <returns></returns>
        [Route("Library/BookList")]
        [HttpGet]
        public IActionResult BookList()
        {
            var bookList = _iLibraryService.GetBookList();

            return View(bookList);
        }

        /// <summary>
        /// جزئییات کتاب
        /// </summary>
        /// <returns></returns>
        [Route("Library/BookDetail/{id}")]
        [HttpGet]
        public IActionResult BookDetail(int id)
        {
            bool checkBookExist = _iLibraryService.CheckBookExistById(id);

            if (checkBookExist == false)
            {
                TempData["BookNotExist"] = "این کتاب موجود نمیباشد";
                return View();
            }

            var book = _iLibraryService.GetBookById(id);

            return View(book);
        }


        /// <summary>
        /// پاک کردن کتاب از کتابخانه
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("Library/DeleteBook/{Id}")]
        [HttpGet]
        public IActionResult DeleteBook(int Id)
        {

            _iLibraryService.Delete(Id);
            TempData["DeleteBook"] = "کتاب با موفقیت حذف شد";

            return RedirectToAction("BookList");
        }

        [Route("Library/Search")]
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [Route("Library/Search")]
        [HttpPost]
        public IActionResult Search(string name)
        {
            if (name == "")
            {
                TempData["EmptyName"] = "فیلد مورد نظر را پر کنید";
                return RedirectToAction();
            }

            var book = _iLibraryService.SearchByName(name);
            TempData["Searched"] = "Done";
            return View("Search", book);
        }

        [Route("Library/AuthorList")]
        [HttpGet]
        public IActionResult AuthorList()
        {
            var authors = _iLibraryService.GetAuthorList();
            return View(authors);
        }

        [Route("Library/DeleteAuthor/{Id}")]
        [HttpGet]
        public IActionResult DeleteAuthor(int Id)
        {
            _iLibraryService.DeleteAuthor(Id);
            TempData["DeleteAuthor"] = "نویسنده با موفقیت حذف شد";

            return RedirectToAction("AuthorList");
        }

    }



    //TODO: پیجینگ برای صفحه بندی راه بندازم
}
