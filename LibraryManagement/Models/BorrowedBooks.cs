using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class BorrowedBooks
    {   [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserEmail { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
