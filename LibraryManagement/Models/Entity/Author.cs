using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models.Entity
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
