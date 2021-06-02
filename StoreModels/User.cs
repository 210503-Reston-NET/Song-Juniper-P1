using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

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
                    throw new InvalidOperationException("Name cannot be empty");
                }
                if (!Regex.IsMatch(value, @"^[A-Za-z .-]+$"))
                {
                    throw new InvalidOperationException("Name is not valid");
                }
                _name = value;
            }
        }

        public List<Order> Orders { get; set; }
    }
}