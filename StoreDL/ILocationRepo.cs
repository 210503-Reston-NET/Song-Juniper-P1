using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreModels;

namespace StoreDL
{
    public interface ILocationRepo
    {
        public List<Location> GetAllLocations();
        public Location GetLocationById(int id);
        public Location GetLocationByName(string name);
        public Location AddNewLocation(Location location);
        public Location UpdateLocation(Location location);
        public List<Inventory> GetLocationInventory(int locationId);
        public Inventory AddInventory(Inventory inventory);
        public Inventory GetInventoryById(int id);
        public Inventory UpdateInventoryItem(Inventory inventory);
        public void DeleteLocation(Location location);
    }
}
