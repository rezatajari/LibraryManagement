using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models.Entity
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set;   }

        public string Year { get; set; }

        public string Price { get; set; }

        public string Description { get; set; }

        public Author Author { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
    }
}
