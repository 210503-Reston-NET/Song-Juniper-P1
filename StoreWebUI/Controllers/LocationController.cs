﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationBL _locationBL;
        private readonly IProductBL _productBL;
        private readonly IOrderBL _orderBL;
        private readonly UserManager<User> _userManager;
        public LocationController(ILocationBL locationBL, IProductBL productBL, IOrderBL orderBL, UserManager<User> userManager)
        {
            _locationBL = locationBL;
            _productBL = productBL;
            _orderBL = orderBL;
            _userManager = userManager;
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

        [Authorize(Roles = "Admin")]
        // GET: LocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(new LocationVM(_locationBL.FindLocationByID(id)));
        }

        // PUT: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View(new LocationVM(_locationBL.FindLocationByID(id)));
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
                    return RedirectToAction(nameof(Inventory), new { id = inventoryVM.LocationId });
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
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateInventory(int id)
        {
            return View(new InventoryVM(_locationBL.GetInventoryById(id)));
        }

        /// <summary>
        /// POST: LocationController/UpdateInventory/5
        /// </summary>
        /// <param name="id">Inventory Id</param>
        /// <param name="inventoryVM">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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

        /// <summary>
        /// GET LocationController/DeleteInventory/5
        /// </summary>
        /// <param name="id">inventory Id</param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteInventory(int id)
        {
            return View(new InventoryVM(_locationBL.GetInventoryById(id)));
        }

        /// <summary>
        /// POST: LocationController/DeleteInventory/5
        /// </summary>
        /// <param name="id">Inventory id</param>
        /// <param name="inventoryVM">submitted form</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteInventory(int id, InventoryVM inventoryVM)
        {
            try
            {
                int LocationId = _locationBL.GetInventoryById(id).LocationId;
                _locationBL.DeleteInventory(new Inventory { Id = id });
                return RedirectToAction(nameof(Inventory), new { id = LocationId });
            }
            catch
            {
                return View(new InventoryVM(_locationBL.GetInventoryById(id)));
            }
        }

        /// <summary>
        /// View all orders placed to This store
        /// </summary>
        /// <param name="id">location Id</param>
        /// <returns>view with list of orders</returns>
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> OrderHistory(int id)
        {
            Location currentLocation = _locationBL.FindLocationByID(id);
            ViewBag.CurrentLocation = currentLocation;
            //grab the orders
            List<Order> orders = _orderBL.GetOrdersByLocationId(id);
            List<OrderVM> orderVMs = new List<OrderVM>();
            foreach(Order ord in orders)
            {
                //only display closed orders
                if (!ord.Closed) break;

                //initialize new orderVM and get all other necessary infos
                OrderVM ordVM = new OrderVM(ord);
                ordVM.OrderUser = await _userManager.FindByIdAsync(ord.UserId.ToString());
                ordVM.LineItems = _orderBL.GetLineItemsByOrderId(ord.Id);
                orderVMs.Add(ordVM);
            }
            return View(orderVMs);
        }
    }
}
