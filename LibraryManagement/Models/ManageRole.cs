using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models
{
    public class ManageRole
    {
        public ManageRole()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
        public bool IsSelected { get; set; }
    }
}
