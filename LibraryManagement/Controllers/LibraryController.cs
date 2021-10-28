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
