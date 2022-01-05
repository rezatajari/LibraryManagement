﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataTransferObjects
{
    public class AddBookDto
    {
        [Required(ErrorMessage = "اسم کتاب را بنویسید")]
        public string BookName { get; set; }

        //[Required(ErrorMessage = "لطفا توضیحاتی برای کتاب بنویسید")]
        public string BookDescription { get; set; } 

        [Required(ErrorMessage = "قیمت کتاب را مشخص کنید")]
        [RegularExpression("^(0|[1-9][0-9]*)$", ErrorMessage = "لطفا قیمت را درست وارد کنید")]
        [StringLength(10, ErrorMessage = "لطفا مقدار صحیح وارد کنید")]
        public string BookPrice { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
