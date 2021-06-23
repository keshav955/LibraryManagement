using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class RegisterUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display (Name = "Confirm Password")]
        [Compare("Password", ErrorMessage ="Password Confirmation Does not Match.")]
        public string ConfirmPassword { get; set; }
    }
}
