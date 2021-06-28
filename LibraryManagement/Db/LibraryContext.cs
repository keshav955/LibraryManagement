using LibraryManagement.Entities;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Db
{
    public class LibraryContext : IdentityDbContext<User> ,ILibraryContext
    {
       public LibraryContext(DbContextOptions<LibraryContext> o) : base(o)
        {

        }
        public DbSet<Bookdetails> bookdetails { get; set; }
        public DbSet<BorrowedBooks> Borrowed { get; set; }
        public void SaveToDb()
        {
            this.SaveChanges();
        }

        /* public DbSet<User> userdetails { get; set; }*/
    }
}
