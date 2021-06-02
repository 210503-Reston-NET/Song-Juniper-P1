using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class LocationRepoDB : ILocationRepo
    {
        private readonly WssDBContext _context;

        public LocationRepoDB(WssDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all locations
        /// </summary>
        /// <returns>List of Location</returns>
        public List<Location> GetAllLocations()
        {
            return _context.Locations
            .AsNoTracking()
            .Select(
                loc => loc
            ).ToList();
        }

        /// <summary>
        /// Searches for location by its id
        /// </summary>
        /// <param name="id">location's id</param>
        /// <returns>found location</returns>
        public Location GetLocationById(int id)
        {
            Location found = _context.Locations
            .AsNoTracking()
            .FirstOrDefault(loc => loc.Id == id);
            return found;
        }

        /// <summary>
        /// Finds location by name
        /// </summary>
        /// <param name="name">string, the name to look for</param>
        /// <returns>the found location</returns>
        public Location GetLocationByName(string name)
        {
            Location found = _context.Locations
            .AsNoTracking()
            .FirstOrDefault(loc => loc.Name == name);
            return found;
        }

        /// <summary>
        /// Creates new location
        /// </summary>
        /// <param name="location">New Location object to be added</param>
        /// <returns>created location</returns>
        public Location AddNewLocation(Location location)
        {
            Location locToAdd = _context.Locations
            .Add(location).Entity;

            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return locToAdd;
        }

        /// <summary>
        /// Updates location info, ie name, address, etc.
        /// </summary>
        /// <param name="location">location object</param>
        /// <returns>updated location object</returns>
        public Location UpdateLocation(Location location)
        {
            Location updated = _context.Locations.Update(location).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return updated;
        }

        /// <summary>
        /// Removes a location
        /// </summary>
        /// <param name="location">location object to be deleted</param>
        public void DeleteLocation(Location location)
        {
            _context.Locations.Remove(location);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        /// <summary>
        /// Gets all inventory items associated to a location by id
        /// Also includes the product detail
        /// </summary>
        /// <param name="locationId">id of the location</param>
        /// <returns>List of inventory items</returns>
        public List<Inventory> GetLocationInventory(int locationId)
        {
            return _context.Inventories.Where(inventory => inventory.LocationId == locationId)
            .AsNoTracking()
            .Include("Product")
            .Select(
                inventory => inventory
            ).ToList();
        }

        /// <summary>
        /// Creates a new associationg between a product to the location
        /// </summary>
        /// <param name="inventory">inventory object with the new product</param>
        /// <returns>added inventory object</returns>
        public Inventory AddInventory(Inventory inventory)
        {
            _context.Inventories.Add(
                inventory
            );
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return inventory;
        }

        /// <summary>
        /// Updates the inventory item. Most commonly used for updating the quantity of particular product associated to this inventory
        /// </summary>
        /// <param name="inventory">Inventory object</param>
        /// <returns>updated inventory object</returns>
        public Inventory UpdateInventoryItem(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return inventory;
        }

        /// <summary>
        /// Finds inventory item by its id
        /// </summary>
        /// <param name="id">id of inventory</param>
        /// <returns>found inventory obj</returns>
        public Inventory GetInventoryById(int id)
        {
            return _context.Inventories
                .AsNoTracking()
                .Include("Product")
                .FirstOrDefault(item => item.Id == id);
        }

        /// <summary>
        /// Deletes inventory
        /// </summary>
        /// <param name="inventory">inventory obj to be deleted</param>
        public void DeleteInventory(Inventory inventory)
        {
            _context.Inventories.Remove(inventory);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}