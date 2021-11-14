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

        [Route("Library/BookList")]
        [HttpGet]
        public IActionResult BookList()
        {
            var bookList = _iLibraryService.GetBookList();

            return View(bookList);
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
