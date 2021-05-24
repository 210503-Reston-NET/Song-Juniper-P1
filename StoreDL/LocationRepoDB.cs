using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class LocationRepoDB : ILocationRepo
    {
        private WssDBContext _context;
        public LocationRepoDB(WssDBContext context)
        {
            _context = context;
        }

        public List<Location> GetAllLocations()
        {
            return _context.Locations
            .AsNoTracking()
            .Select(
                loc => loc
            ).ToList();
        }

        public Location GetLocationById(int id)
        {
            Location found = _context.Locations
            .AsNoTracking()
            .FirstOrDefault(loc => loc.Id == id);
            return found;
        }

        public Location GetLocationByName(string name)
        {
            Location found = _context.Locations
            .AsNoTracking()
            .FirstOrDefault(loc => loc.Name == name);
            return found;
        }

        public Location AddNewLocation(Location loc)
        {
            Location locToAdd = _context.Locations
            .Add(loc).Entity;
            
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return locToAdd;
        }

        public Location UpdateLocation(Location location)
        {
            Location updated = _context.Locations.Update(location).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return updated;
        }

        public void DeleteLocation(Location location)
        {
            _context.Locations.Remove(location);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public List<Inventory> GetLocationInventory(int locationId)
        {
            return _context.Inventories.Where(inventory => inventory.LocationId == locationId)
            .AsNoTracking()
            .Include("Product")
            .Select(
                inventory => inventory
            ).ToList();

        }

        public Inventory AddInventory(Inventory inventory)
        {
            _context.Inventories.Add(
                inventory
            );
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return inventory;
        }

        public Inventory UpdateInventoryItem(Inventory inventory)
        {
            Inventory toUpdate = _context.Inventories
            .FirstOrDefault(inven => inven.Id == inventory.Id);
            toUpdate.Quantity = inventory.Quantity;
            
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return inventory;
        }
    }
}