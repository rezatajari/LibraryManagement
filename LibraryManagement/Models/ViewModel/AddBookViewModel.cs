using LibraryManagement.DataTransferObjects;
using LibraryManagement.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.ViewModel
{
    public class AddBookViewModel
    {
        public MessageContract<List<AuthorView>> AuthorListMessageContract { get; set; }

        public AddBookDto AddBookDto { get; set; }
    }
}
