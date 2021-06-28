using LibraryManagement.Db;
using LibraryManagement.Entities;
using LibraryManagement.Models;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Services
{
    public class LibraryServices :ILibraryServices
    {
        private LibraryContext libraryContext ;
        public LibraryServices(LibraryContext libraryContext)
        {
            this.libraryContext = libraryContext;
        }

        public void CreateBooks(string title, string author, string publisher, string discription, bool available)
        {
            Bookdetails newBook = new Entities.Bookdetails
            {
                Title = title,
                Auther = author,
                Publisher = publisher,
                Discription = discription,
                Available = available
            };
            libraryContext.bookdetails.Add(newBook);
            libraryContext.SaveToDb();
        }
        public void IssueBook(int BookId, string UserEmail, DateTime date, DateTime returndate)
        {
            BorrowedBooks newBorrowreq = new BorrowedBooks
            {
                UserEmail = UserEmail,
                BookId = BookId,
                BookingDate = date,
                ReturnDate = returndate
            };
            libraryContext.Borrowed.Add(newBorrowreq);
            libraryContext.SaveToDb();
        }

        public List<Bookdetails> GetBooks()
        {
            return libraryContext.bookdetails.ToList();
        }
        public List<BorrowedBooks> GetIssued()
        {
            return libraryContext.Borrowed.ToList();
            
        }

        public Bookdetails GetBookById(int id)
        {
           var book = libraryContext.bookdetails.FirstOrDefault(a => a.Id.Equals(id));
           return book;
        }
    }
}
