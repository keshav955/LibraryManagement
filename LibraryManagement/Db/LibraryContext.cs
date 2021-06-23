using LibraryManagement.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Db
{
    public class LibraryContext : IdentityDbContext<User> ,IlibraryContext
    {
       public LibraryContext(DbContextOptions<LibraryContext> o) : base(o)
        {

        }

       /* public DbSet<User> userdetails { get; set; }*/
    }
}
