using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Entities
{
    public class Bookdetails
    {
        public int Id { get; set; }
        public string Publisher { get; set; }
        public string Auther { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public bool Available { get; set; }
       
        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
       // [Required(ErrorMessage = "Please choose file to upload.")]
        public string Image { get; set; }

    }
}
