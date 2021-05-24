using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreModels;

namespace StoreBL
{
    public interface ILocationBL
    {
        public Location AddNewLocation(Location location);
        public List<Location> GetAllLocations();
        public Location FindLocationByName(string name);
        public Location FindLocationByID(int id);
        public List<Inventory> GetLocationInventory(int locId);
        public Inventory AddInventory(Inventory inventory);
        public Inventory UpdateInventoryItem(Inventory inventory);
        public Inventory GetInventoryById(int id);
        public Location UpdateLocation(Location location);

        public void DeleteLocation(Location location);
    }
}
