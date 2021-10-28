using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.ViewModelDto
{
    public class AddBookDto
    {
        public string BookName { get; set; }

        public string BookDescription { get; set; }

        public string BookPrice { get; set; }

        public string AuthorName { get; set; }

        public int AuthorAge { get; set; }
    }
}
