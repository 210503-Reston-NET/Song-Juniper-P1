using System;
using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    public class LocationBL
    {
        private LocationRepoDB _repo;
        public LocationBL(LocationRepoDB repo)
        {
            _repo = repo;
        }

        public Location AddNewLocation(Location newLoc)
        {
            if(FindLocationByName(newLoc.Name) is not null)
            {
                throw new Exception("There is already a location with the same name");
            }
            return _repo.AddNewLocation(newLoc);
        }

        // public void RemoveLocation(Location location)
        // {
        //     _repo.RemoveLocation(location);
        // }
        public List<Location> GetAllLocations()
        {
            return _repo.GetAllLocations();
        }

        public Location FindLocationByName(string name)
        {
            return _repo.GetLocationByName(name);
        }

        public Location FindLocationByID(int id)
        {
            return _repo.GetLocationById(id);
        }

        public List<Inventory> GetLocationInventory(int id)
        {
            return _repo.GetLocationInventory(id);
        }
        public Inventory AddInventory(Inventory inventory)
        {
            return _repo.AddInventory(inventory);
        }

        public Inventory UpdateInventoryItem(Inventory inventory)
        {
            return _repo.UpdateInventoryItem(inventory);
        }
    }
}
