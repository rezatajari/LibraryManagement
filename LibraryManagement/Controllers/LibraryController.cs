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

            bool checkExistBook = _iLibraryService.CheckThereSameBook(newBookDto.BookName, newBookDto.AuthorName);

            if (checkExistBook == true)
            {
                TempData["ExistBook"] = "این کتاب قبلا ثبت شده است";
                return RedirectToAction("AddBook");
            }

            _iLibraryService.Add(newBookDto);
            TempData["AddBook"] = "کتاب با موفقیت ثبت شد";

            return RedirectToAction("AddBook");
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
    }



    //TODO: پیجینگ برای صفحه بندی راه بندازم
}
