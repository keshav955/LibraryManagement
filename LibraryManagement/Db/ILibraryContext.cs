using LibraryManagement.Entities;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Db
{
    public interface ILibraryContext
    {
         DbSet<Bookdetails> bookdetails { get; set; }
         DbSet<BorrowedBooks> Borrowed { get; set; }
        void SaveToDb();
    }
}
