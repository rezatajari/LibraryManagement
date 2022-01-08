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

        /// <summary>
        /// نمایش صفحه اضافه کردن کتاب به کتابخانه
        /// </summary>
        /// <returns></returns>
        [Route("Library/AddBook")]
        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            var result = await _iLibraryService.GetAuthorList();
            ViewBag.AuthorList = result;

            return View();
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

            // گرفتن اسم نویسنده بوسیله آیدی دریافتی از ویو
            var authorResponse = await _iLibraryService.GetAuthorNameById(newBookDto.AuthorId);
            string authorName = authorResponse.Data;
            string bookName = newBookDto.BookName;

            if (authorResponse.IsSuccess == false)
            {
                TempData["ExistAuthor"] = authorResponse.Message;
                return RedirectToAction("AddBook");
            }

            MessageContract<bool> checkExistBookResponse = await _iLibraryService.CheckThereSameBook(bookName, authorName);

            if (checkExistBookResponse.Data == true)
            {
                TempData["ExistBook"] = checkExistBookResponse.Message;
                return RedirectToAction("AddBook");
            }

            MessageContract resultAddBook = await _iLibraryService.Add(newBookDto);
            TempData["AddBook"] = resultAddBook.Message;

            return RedirectToAction("AddBook");
        }

        /// <summary>
        /// صفحه نمایش اضافه کردن نویسنده 
        /// </summary>
        /// <returns></returns>
        [Route("Library/AddAuthor")]
        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }

        /// <summary>
        /// اضافه کردن نویشنده
        /// </summary>
        /// <param name="newAuthorDto"></param>
        /// <returns></returns>
        [Route("Library/AddAuthor")]
        [HttpPost]
        public async Task<IActionResult> AddAuthor(AddAuthorDto newAuthorDto)
        {

            if (!ModelState.IsValid)
                return View();

            MessageContract<bool> resultCheckAuthorExist = await _iLibraryService.CheckAuthorExistByName(newAuthorDto.Name);

            if (resultCheckAuthorExist.Data == true)
            {
                TempData["ExistAuthor"] = resultCheckAuthorExist.Message;
                return RedirectToAction("AddAuthor");
            }

            MessageContract resultAddAuthor = await _iLibraryService.AddAuthor(newAuthorDto);

            if (resultAddAuthor.IsSuccess)
            {

                TempData["AddAuthor"] = resultAddAuthor.Message;
                return RedirectToAction("AddAuthor");
            }

            return View();

        }

        /// <summary>
        /// لیست کلیه کتاب های داخل کتابخانه 
        /// </summary>
        /// <returns></returns>
        [Route("Library/BookList")]
        [HttpGet]
        public async Task<IActionResult> BookList()
        {
            MessageContract<List<BookListDto>> resultBookList = await _iLibraryService.GetBookList();

            return View(resultBookList.Data);
        }

        /// <summary>
        /// جزئییات کتاب
        /// </summary>
        /// <returns></returns>
        [Route("Library/BookDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> BookDetail(int id)
        {
            MessageContract<bool> resultCheckBookExist = await _iLibraryService.CheckBookExistById(id);

            if (resultCheckBookExist.Data == false)
            {
                TempData["BookNotExist"] = resultCheckBookExist.Message;
                return View();
            }

            MessageContract<BookDetailDto> resultBookDetail = await _iLibraryService.GetBookById(id);

            return View(resultBookDetail.Data);
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

            MessageContract resultDeleteBook = await _iLibraryService.Delete(Id);
            TempData["DeleteBook"] = resultDeleteBook.Message;

            return RedirectToAction("BookList");
        }

        /// <summary>
        /// صفحه نمایش جستجو کردن
        /// </summary>
        /// <returns></returns>
        [Route("Library/Search")]
        [HttpGet]
        public IActionResult Search(MessageContract<BookListDto> messageContract)
        {
            if (messageContract.IsSuccess == false && messageContract.Message != null)
            {
                messageContract = new MessageContract<BookListDto>();
                return View(messageContract);
            }

            return View(messageContract);

        }

        /// <summary>
        /// جستجو کردن
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("Library/Search")]
        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            if (name == "")
            {
                TempData["EmptyName"] = "فیلد مورد نظر را پر کنید";
                return RedirectToAction();
            }

            MessageContract<BookListDto> resultSearch = await _iLibraryService.SearchByName(name);

            if (resultSearch.IsSuccess)
                return View("Search", resultSearch);

            return View(resultSearch);
        }

        /// <summary>
        /// نمایش کلیه نویسنده ها
        /// </summary>
        /// <returns></returns>
        [Route("Library/AuthorList")]
        [HttpGet]
        public async Task<IActionResult> AuthorList()
        {
            MessageContract<List<AuthorView>> resultAuthorView = await _iLibraryService.GetAuthorList();

            if (resultAuthorView.IsSuccess)
                return View(resultAuthorView.Data);

            TempData["NotAuthorListExist"] = resultAuthorView.Message;
            return View();

        }

        /// <summary>
        /// حذف کردن نویسنده
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("Library/DeleteAuthor/{Id}")]
        [HttpGet]
        public async Task<IActionResult> DeleteAuthor(int Id)
        {
            MessageContract resultDeleteAuthor = await _iLibraryService.DeleteAuthor(Id);

            if (resultDeleteAuthor.IsSuccess)
                TempData["DeleteAuthor"] = resultDeleteAuthor.Message;

            return RedirectToAction("AuthorList");
        }

        /// <summary>
        /// جزئییات نویسنده
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("Library/AuthorDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> AuthorDetail(int Id)
        {
            MessageContract<bool> resultAuthorExist = await _iLibraryService.CheckAuthorExistById(Id);

            if (resultAuthorExist.IsSuccess == false)
            {
                TempData["AuthorNotExist"] = resultAuthorExist.Message;
                return View();
            }

            MessageContract<AuthorDetailDto> resultAuthorDetail = await _iLibraryService.GetAuthorById(Id);
            if (resultAuthorDetail.IsSuccess)
                return View(resultAuthorExist.Data);

            TempData["AuthorNotExist"] = resultAuthorDetail.Message;
            return View();
        }
    }

    //TODO: پیجینگ برای صفحه بندی راه بندازم
}
