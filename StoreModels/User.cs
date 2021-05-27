using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StoreModels
{
    class User : IdentityUser
    {
        public User() { }
        public string Name { get; set; }
        public IdentityRole Role { get; set; }

    }
}
