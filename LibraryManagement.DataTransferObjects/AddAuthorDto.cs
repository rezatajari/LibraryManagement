using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DataTransferObjects
{
    public class AddAuthorDto
    {
        [Required(ErrorMessage = "اسم کتاب را بنویسید")]
        public string Name { get; set; }

        [Required(ErrorMessage = "سن نویسنده را مشخص کنید")]
        [StringLength(2, ErrorMessage = "لطفا مقدار صحیح وارد کنید")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "لطفا سن را درست وارد کنید")]
        public string Age { get; set; }
    }
}
