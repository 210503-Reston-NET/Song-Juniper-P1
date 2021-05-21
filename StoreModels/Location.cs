using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a store location.
    /// </summary>
    public class Location
    {   
        public Location() {}
        public Location(string name, string address)
        {
            this.Name = name;
            this.Address = address;
        }
        
        public Location(int id, string name, string address) : this(name, address)
        {
            this.Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public List<Inventory> Inventory { get; set; }

        public List<Order> Orders { get; set; }

        public override string ToString()
        {
            return $"Name: {this.Name} \nAddress: {this.Address}";
        }

        // override object.Equals
        public bool Equals(Location loc)
        {    
            if(loc == null) return false;
            return this.Name == loc.Name && this.Address == loc.Address;
        }
    }
}