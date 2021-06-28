using LibraryManagement.Entities;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Services
{
   public interface ILibraryServices
    {
        void CreateBooks(string title, string author, string publisher, string discription, bool available);

        void IssueBook(int BookId ,string UserEmail , DateTime date, DateTime returndate);
        List<Bookdetails> GetBooks();
        List<BorrowedBooks> GetIssued();
        Bookdetails GetBookById(int id);

    }
}
