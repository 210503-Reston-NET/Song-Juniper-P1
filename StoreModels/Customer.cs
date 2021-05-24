using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace StoreModels
{
    /// <summary>
    /// This class should contain necessary properties and fields for customer info.
    /// </summary>
    public class Customer
    {
        private string _name;
        public Customer() {}
        public Customer (string name, string email)
        {
            this.Name = name;
            this.Email = email;
        }

        public Customer (int id, string name, string email) : this(name, email)
        {
            this.Id = id;
        }

        public Customer (int id, string name, string email, List<Order> orders) : this(id, name, email)
        {
            this.Orders = orders;
        }

        public Customer (int id)
        {
            this.Id = id;
        }

        public string Name 
        {
            get { return _name; }
            set 
            {
                if(value.Length == 0)
                {
                    throw new Exception("Name cannot be empty");
                }
                if(!Regex.IsMatch(value, @"^[A-Za-z .-]+$"))
                {
                    throw new Exception("Name is not valid");
                }
                _name = value;
            } 
        }

        public int Id { get; set; }

        public string Email { get; set; }


        public List<Order> Orders { get; set; }

        public override string ToString()
        {
            return $"Name: {this.Name}";
        }

    }
}