using AutoMapper;
using LibraryManagement.DataTransferObjects;
using LibraryManagement.Entities;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public async Task<IActionResult> AddBook()
        {
            var newBookDto = new AddBookDto();
            ViewBag.AuthorList = await _iLibraryService.GetAuthorList();
            return View(newBookDto);
        }

        /// <summary>
        /// اضافه کردن کتاب به کتابخانه
        /// </summary>
        /// <param name="newBookDto"></param>
        /// <returns></returns>
        [Route("Library/AddBook")]
        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookDto newBookDto)
        {
            if (!ModelState.IsValid)
                return View();

            if (newBookDto == null)
                throw new ArgumentNullException();

            string authorName = await _iLibraryService.GetAuthorNameById(newBookDto.AuthorId);

            bool checkExistBook = await _iLibraryService.CheckThereSameBook(newBookDto.BookName, authorName);

            if (checkExistBook == true)
            {
                TempData["ExistBook"] = "این کتاب قبلا ثبت شده است";
                return RedirectToAction("AddBook");
            }

            await _iLibraryService.Add(newBookDto);
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
        public async Task<IActionResult> AddAuthor(AddAuthorDto newAuthorDto)
        {

            if (!ModelState.IsValid)
                return View();

            bool checkExistAuthorName = await _iLibraryService.CheckAuthorExistByName(newAuthorDto.Name);

            if (checkExistAuthorName == true)
            {
                TempData["ExistAuthor"] = "این نویسنده قبلا ثبت شده است";
                return RedirectToAction("AddAuthor");
            }

            await _iLibraryService.AddAuthor(newAuthorDto);
            TempData["AddAuthor"] = "نویسنده با موفقیت ثبت شد";

            return RedirectToAction("AddAuthor");

        }

        /// <summary>
        /// لیست کلیه کتاب های داخل کتابخانه 
        /// </summary>
        /// <returns></returns>
        [Route("Library/BookList")]
        [HttpGet]
        public async Task<IActionResult> BookList()
        {
            var bookList = await _iLibraryService.GetBookList();

            return View(bookList);
        }

        /// <summary>
        /// جزئییات کتاب
        /// </summary>
        /// <returns></returns>
        [Route("Library/BookDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> BookDetail(int id)
        {
            bool checkBookExist = await _iLibraryService.CheckBookExistById(id);

            if (checkBookExist == false)
            {
                TempData["BookNotExist"] = "این کتاب موجود نمیباشد";
                return View();
            }

            var book = await _iLibraryService.GetBookById(id);

            return View(book);
        }

        /// <summary>
        /// پاک کردن کتاب از کتابخانه
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("Library/DeleteBook/{Id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteBook(int Id)
        {

            await _iLibraryService.Delete(Id);
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
        public async Task<IActionResult> Search(string name)
        {
            if (name == "")
            {
                TempData["EmptyName"] = "فیلد مورد نظر را پر کنید";
                return RedirectToAction();
            }

            var book = await _iLibraryService.SearchByName(name);
            TempData["Searched"] = "Done";
            return View("Search", book);
        }

        [Route("Library/AuthorList")]
        [HttpGet]
        public async Task<IActionResult> AuthorList()
        {
            var authors = await _iLibraryService.GetAuthorList();
            return View(authors);
        }

        [Route("Library/DeleteAuthor/{Id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteAuthor(int Id)
        {
            await _iLibraryService.DeleteAuthor(Id);
            TempData["DeleteAuthor"] = "نویسنده با موفقیت حذف شد";

            return RedirectToAction("AuthorList");
        }

        [Route("Library/AuthorDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> AuthorDetail(int Id)
        {
            bool checkAuthorExist = await _iLibraryService.CheckAuthorExistById(Id);

            if (checkAuthorExist == false)
            {
                TempData["AuthorNotExist"] = "این نویسنده موجود نمی باشد";
                return View();
            }

            AuthorDetailDto authorDetailDto = await _iLibraryService.GetAuthorById(Id);

            return View(authorDetailDto);
        }
    }

    //TODO: پیجینگ برای صفحه بندی راه بندازم
}
