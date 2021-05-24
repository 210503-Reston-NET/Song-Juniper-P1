using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class LocationController : Controller
    {
        private ILocationBL _locationBL;
        private IProductBL _productBL;
        public LocationController(ILocationBL locationBL, IProductBL productBL)
        {
            _locationBL = locationBL;
            _productBL = productBL;
        }
        // GET: LocationController
        public ActionResult Index()
        {
            return View(
                _locationBL.GetAllLocations()
                .Select(location => new LocationVM(location))
                .ToList()
                );
        }

        // GET: LocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocationVM locationVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _locationBL.AddNewLocation(new Location
                    {
                        Name = locationVM.Name,
                        Address = locationVM.Address,
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(new LocationVM(_locationBL.FindLocationByID(id)));
        }

        // PUT: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocationVM locationVM)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _locationBL.UpdateLocation(new Location
                    {
                        Id = locationVM.Id,
                        Name = locationVM.Name,
                        Address = locationVM.Address
                    }); ;
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(new LocationVM(_locationBL.FindLocationByID(id)));
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LocationVM locationVM)
        {
            try
            {
                _locationBL.DeleteLocation(new Location
                {
                    Id = locationVM.Id,
                    Name = locationVM.Name,
                    Address = locationVM.Address
                });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // GET: LocationController/Inventory/5
        public ActionResult Inventory(int id)
        {
            LocationVM location = new LocationVM(_locationBL.FindLocationByID(id));
            location.Inventories = _locationBL.GetLocationInventory(id);
            return View(location);
        }

        // GET: LocationController/AddInventory/5
        public ActionResult AddInventory(int id)
        {
            //first, get the current inventory of the store, and just pluck the produt id's so we can compare them later
            List<int> currentInventoryProductId = _locationBL.GetLocationInventory(id).Select(item => item.Product.Id).ToList();
            //and initialize the new inventoryVM instance
            InventoryVM newInventory = new InventoryVM();
            newInventory.LocationId = id;

            //And then initialize the product options
            newInventory.ProductOptions = new List<SelectListItem>();

            //then, grab all products so we can fill out the select by looping over the products and inserting it to the product options list
            List <Product> allProducts= _productBL.GetAllProducts();
            foreach(Product prod in allProducts)
            {
                //Only add it to the option if it's not already in the inventory
                int alreadyExists = currentInventoryProductId.FindIndex(id => id == prod.Id);
                if (alreadyExists == -1)
                    {
                        SelectListItem listItem = new SelectListItem
                    {
                        Text = prod.Name,
                        Value = prod.Id.ToString()
                    };
                    newInventory.ProductOptions.Add(listItem);
                }
            }
            return View(newInventory);
        }

        // POST: LocationController/AddInventory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddInventory(int id, InventoryVM inventoryVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _locationBL.AddInventory(new Inventory {
                        ProductId = inventoryVM.ProductId,
                        LocationId = id,
                        Quantity = inventoryVM.Quantity
                    }); ;
                    return RedirectToAction(nameof(Index));
                }
                return AddInventory(id);
            }
            catch
            {
                return AddInventory(id);
            }
        }

        /// <summary>
        /// GET: LocationController/UpdateInventory/5
        /// </summary>
        /// <param name="id">id of inventory to be updated</param>
        /// <returns></returns>
        public ActionResult UpdateInventory(int id)
        {
            return View(new InventoryVM(_locationBL.GetInventoryById(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInventory(int id, InventoryVM inventoryVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _locationBL.UpdateInventoryItem(new Inventory {
                        Id = inventoryVM.Id,
                        ProductId = inventoryVM.ProductId,
                        LocationId = inventoryVM.LocationId,
                        Quantity = inventoryVM.Quantity
                    });
                    return RedirectToAction(nameof(Inventory), new { id = inventoryVM.LocationId});
                }
                return UpdateInventory(id);
            }
            catch
            {
                return UpdateInventory(id);
            }
        }
    }
}
