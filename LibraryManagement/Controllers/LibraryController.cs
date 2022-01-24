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

            if (result.Errors.Count != 0)
            {
                ViewBag.AuthorListErrors = result.Errors;
                return View();
            }

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
            if (authorResponse.Errors.Count != 0)
            {
                ViewBag.AuthorNameErrors = authorResponse.Errors;
                return View();
            }

            string authorName = authorResponse.Data;
            string bookName = newBookDto.BookName;

            if (authorResponse.IsSuccess == false)
            {
                TempData["ExistAuthor"] = authorResponse.Message;
                return RedirectToAction("AddBook");
            }

            MessageContract<bool> checkExistBookResponse = await _iLibraryService.CheckThereSameBook(bookName, authorName);
            if (checkExistBookResponse.Errors != null)
            {
                ViewBag.ExistBookErrors = checkExistBookResponse.Errors;
                return View();
            }

            if (checkExistBookResponse.Data == true)
            {
                TempData["ExistBook"] = checkExistBookResponse.Message;
                return RedirectToAction("AddBook");
            }

            MessageContract resultAddBook = await _iLibraryService.Add(newBookDto);
            if (resultAddBook.IsSuccess == false)
            {
                ViewBag.Errors = resultAddBook.Errors;
                return View("AddBook");
            }

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
            if (resultCheckAuthorExist.Errors.Count != 0)
            {
                ViewBag.AuthorExistErrors = resultCheckAuthorExist.Errors;
                return View();
            }

            if (resultCheckAuthorExist.Data == true)
            {
                TempData["ExistAuthor"] = resultCheckAuthorExist.Message;
                return RedirectToAction("AddAuthor");
            }

            MessageContract resultAddAuthor = await _iLibraryService.AddAuthor(newAuthorDto);
            if (resultAddAuthor.Errors.Count != 0)
            {
                ViewBag.AddAuthorErrors = resultAddAuthor.Errors;
                return View();
            }

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
            if (resultBookList.Errors.Count!= 0)
            {
                ViewBag.BookListErros = resultBookList.Errors;
                return View();
            }

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

            if (resultCheckBookExist.Errors.Count != 0)
            {
                ViewBag.BookExistErrors = resultCheckBookExist.Errors;
                return View();
            }

            MessageContract<BookDetailDto> resultBookDetail = await _iLibraryService.GetBookById(id);
            if (resultBookDetail.Errors.Count != 0)
            {
                ViewBag.BookDetailErrors = resultBookDetail.Errors;
                return View();
            }

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
            if (resultDeleteBook.Errors.Count != 0)
            {
                ViewBag.DelBookErrors = resultDeleteBook.Errors;
                return RedirectToAction("BookList");
            }
            TempData["DeleteBook"] = resultDeleteBook.Message;

            return RedirectToAction("BookList");
        }

        /// <summary>
        /// صفحه نمایش جستجو کردن
        /// </summary>
        /// <returns></returns>
        [Route("Library/Search")]
        [HttpGet]
        public IActionResult Search()
        {
            return View();

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
            if (resultSearch.Errors.Count != 0)
            {
                ViewBag.SearchErrors = resultSearch.Errors;
                return View();
            }

            if (resultSearch.IsSuccess)
            {
                ViewBag.SearchResult = resultSearch.Data;
                return View("Search");
            }

            TempData["NotFound"] = resultSearch.Message;
            return View("Search");
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

            if (resultAuthorView.Errors.Count != 0)
            {
                ViewBag.AuthorListErrors = resultAuthorView.Errors;
                return View();
            }

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
            if (resultDeleteAuthor.Errors.Count != 0)
            {
                ViewBag.DelAuthorErrors = resultDeleteAuthor.Errors;
                return RedirectToAction("AuthorList");
            }

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
            if (resultAuthorExist.Errors.Count != 0)
            {
                ViewBag.AuthorExistErrors = resultAuthorExist.Errors;
                return View();
            }

            if (resultAuthorExist.IsSuccess == false)
            {
                TempData["AuthorNotExist"] = resultAuthorExist.Message;
                return View();
            }

            MessageContract<AuthorDetailDto> resultAuthorDetail = await _iLibraryService.GetAuthorById(Id);
            if (resultAuthorDetail.Errors.Count != 0)
            {
                ViewBag.AuthorDetailErrors = resultAuthorDetail.Errors;
                return View();
            }

            if (resultAuthorDetail.IsSuccess)
                return View(resultAuthorExist.Data);

            TempData["AuthorNotExist"] = resultAuthorDetail.Message;
            return View();
        }
    }

    //TODO: پیجینگ برای صفحه بندی راه بندازم
}
