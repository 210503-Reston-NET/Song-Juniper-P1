using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using StoreModels;

namespace StoreWebUI.Models
{
    public class LocationVM
    {
        public LocationVM(Location location)
        {
            Id = location.Id;
            Name = location.Name;
            Address = location.Address;
        }

        public LocationVM()
        {
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public List<Inventory> Inventories { get; set; }
    }
}