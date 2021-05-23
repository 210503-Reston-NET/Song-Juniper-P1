using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Inventory> Inventories { get; set; }
        public List<Order> Orders { get; set; }
    }
}
