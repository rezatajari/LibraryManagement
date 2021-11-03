﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Web.Models.ViewModelDto
{
    public class BookListDto
    {
        public int Id { get; set; }

        public string Book { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }
    }
}
