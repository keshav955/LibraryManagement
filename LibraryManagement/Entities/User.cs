using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Entities
{
    public class User : IdentityUser//generic
    {
        public bool IsApproved { get; set; }
    }
}
