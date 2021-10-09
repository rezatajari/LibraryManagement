using LibraryManagement.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models.DataBaseContext
{
    public class LibraryDatabase : DbContext
    {
        public LibraryDatabase(DbContextOptions<LibraryDatabase> opt)
        : base(opt)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }
    }
}
