using System;
using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    /// <summary>
    /// business logic class for all methods that deals with locations and its inventory
    /// </summary>
    public class LocationBL : ILocationBL
    {
        private ILocationRepo _repo;
        public LocationBL(ILocationRepo repo)
        {
            _repo = repo;
        }
        /// <summary>
        /// First tries to see if there is already a location with the same name. If not, calls repo method for creating a new location
        /// </summary>
        /// <param name="newLoc">Location object</param>
        /// <returns>created location object</returns>
        public Location AddNewLocation(Location newLoc)
        {
            if(FindLocationByName(newLoc.Name) is not null)
            {
                throw new Exception("There is already a location with the same name");
            }
            return _repo.AddNewLocation(newLoc);
        }

        /// <summary>
        /// calls repo method for getting all locations
        /// </summary>
        /// <returns>list of locations</returns>
        public List<Location> GetAllLocations()
        {
            return _repo.GetAllLocations();
        }

        /// <summary>
        /// calls repo method for finding the location by name
        /// </summary>
        /// <param name="name">string, location name</param>
        /// <returns>location if found, null otherwise</returns>
        public Location FindLocationByName(string name)
        {
            return _repo.GetLocationByName(name);
        }
        /// <summary>
        /// calls repo method to search for a location by id
        /// </summary>
        /// <param name="id">integer, id</param>
        /// <returns>location if found, null otherwise</returns>
        public Location FindLocationByID(int id)
        {
            return _repo.GetLocationById(id);
        }
        /// <summary>
        /// calls a repo method that gets all the inventory that is associated with the location id
        /// </summary>
        /// <param name="id">int, location Id</param>
        /// <returns>list of inventory objects associated to the location</returns>
        public List<Inventory> GetLocationInventory(int id)
        {
            return _repo.GetLocationInventory(id);
        }
        /// <summary>
        /// calls the repo method to add new product to the location's inventory
        /// </summary>
        /// <param name="inventory">inventory object</param>
        /// <returns>created inventory object</returns>
        public Inventory AddInventory(Inventory inventory)
        {
            return _repo.AddInventory(inventory);
        }
        /// <summary>
        /// this is for updating the quantity of the already existing inventory product in a location
        /// </summary>
        /// <param name="inventory">inventory object</param>
        /// <returns>updated inventory object</returns>
        public Inventory UpdateInventoryItem(Inventory inventory)
        {
            return _repo.UpdateInventoryItem(inventory);
        }
        /// <summary>
        /// calls repo method to update location details (ie, name, address, etc)
        /// </summary>
        /// <param name="location">location object</param>
        /// <returns>location updated</returns>
        public Location UpdateLocation(Location location)
        {
            return _repo.UpdateLocation(location);
        }

        /// <summary>
        /// calls the repo method to delete the location
        /// </summary>
        /// <param name="location">location object</param>
        public void DeleteLocation(Location location)
        {
            _repo.DeleteLocation(location);
        }
    }
}
