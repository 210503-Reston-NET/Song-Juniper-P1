using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace StoreModels
{
    public class User : IdentityUser<Guid>
    {
        private string _name;

        [PersonalData]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length == 0)
                {
                    throw new Exception("Name cannot be empty");
                }
                if (!Regex.IsMatch(value, @"^[A-Za-z .-]+$"))
                {
                    throw new Exception("Name is not valid");
                }
                _name = value;
            }
        }

        public List<Order> Orders { get; set; }
    }
}
