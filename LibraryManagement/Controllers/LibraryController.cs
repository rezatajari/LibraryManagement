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

        [Route("Library/Add")]
        [HttpGet]
        public IActionResult Add()
        {
            var newBook = new BookDto();
            return View(newBook);
        }

        [Route("Library/Add")]
        [HttpPost]
        public IActionResult Add(BookDto newBook)
        {
            return View();
        }

        [Route("Library/Edit")]
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [Route("Library/Delete")]
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }


    }
}
