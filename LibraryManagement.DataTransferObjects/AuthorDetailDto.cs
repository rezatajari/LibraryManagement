using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataTransferObjects
{
    public class AuthorDetailDto
    {
        public string Name { get; set; }

        public string Age { get; set; }

        public int BookCount { get; set; }
    }
}
