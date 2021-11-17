using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataTransferObjects
{
    public class BookDetailDto
    {
        public string Book { get; set; }

        public string Author { get; set; }

        public string Price { get; set; }

        public string Description { get; set; }
    }
}
