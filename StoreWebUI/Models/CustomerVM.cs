using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StoreModels;

namespace StoreWebUI.Models
{
    /// <summary>
    /// View Model for the Customer Class
    /// </summary>
    public class CustomerVM
    {
        public CustomerVM() { }
        public CustomerVM(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public int Id { get; set; }
    }
}
