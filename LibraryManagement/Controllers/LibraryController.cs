using LibraryManagement.Service;
using LibraryManagement.Web.Models;
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
            var newBook = new BookDto();
            return View(newBook);
        }

        [Route("Library/AddBook")]
        [HttpPost]
        public IActionResult AddBook(BookDto newBook)
        {
            return View();
        }

        [Route("Library/EditBook")]
        [HttpGet]
        public IActionResult EditBook()
        {
            return View();
        }

        [Route("Library/DeleteBook")]
        [HttpGet]
        public IActionResult DeleteBook()
        {
            return View();
        }


    }
}
