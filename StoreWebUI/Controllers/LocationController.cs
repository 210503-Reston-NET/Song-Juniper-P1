using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;
using Serilog;

namespace StoreWebUI.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationBL _locationBL;
        private readonly IProductBL _productBL;
        private readonly IOrderBL _orderBL;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILocationBL locationBL, IProductBL productBL, IOrderBL orderBL, UserManager<User> userManager, ILogger<LocationController> logger)
        {
            _locationBL = locationBL;
            _productBL = productBL;
            _orderBL = orderBL;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: LocationController
        public ActionResult Index()
        {
            _logger.LogInformation("Get: Location Controller Index");
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
            _logger.LogInformation("Get: Location Controller/Create New Location");
            return View();
        }

        // POST: LocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(LocationVM locationVM)
        {
            _logger.LogInformation("POST: LocationController/Create Creating New Location", locationVM);
            try
            {
                if (ModelState.IsValid)
                {
                    _locationBL.AddNewLocation(new Location
                    {
                        Name = locationVM.Name,
                        Address = locationVM.Address,
                    });
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "POST: CREATE Location Failed", locationVM);
                return View();
            }
        }

        // GET: LocationController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            _logger.LogInformation("GET: LocationController/Edit/id Editing Location, ID: ", id);

            return View(new LocationVM(_locationBL.FindLocationByID(id)));
        }

        // PUT: LocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, LocationVM locationVM)
        {
            _logger.LogInformation("POST: LocationController/Edit/id Editing Location", locationVM);
            try
            {
                if (ModelState.IsValid)
                {
                    _locationBL.UpdateLocation(new Location
                    {
                        Id = locationVM.Id,
                        Name = locationVM.Name,
                        Address = locationVM.Address
                    });
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "POST: EDIT Location Failed", locationVM);
                return View();
            }
        }

        // GET: LocationController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _logger.LogInformation("GET: LocationController/Delete/id Deleting a Location id: ", id);
            return View(new LocationVM(_locationBL.FindLocationByID(id)));
        }

        // POST: LocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, LocationVM locationVM)
        {
            _logger.LogInformation("POST: LocationController/Delete/id Deleting a Location Id: ", id);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "POST: Delete Location Failed", locationVM);
                return View();
            }
        }

        /// <summary>
        /// GET: LocationController/Inventory/5
        /// </summary>
        /// <param name="id">Location Id</param>
        /// <returns>View of all inventories in the location</returns>
        public ActionResult Inventory(int id)
        {
            _logger.LogInformation("GET: LocationController/Inventory/id Getting inventories of a location Id:", id);
            LocationVM location = new LocationVM(_locationBL.FindLocationByID(id));
            location.Inventories = _locationBL.GetLocationInventory(id);
            return View(location);
        }

        // GET: LocationController/AddInventory/5
        [Authorize(Roles = "Admin")]
        public ActionResult AddInventory(int id)
        {
            _logger.LogInformation("GET: LocationController/AddInventory/id, Adding a new inventory to the location Id: ", id);
            //first, get the current inventory of the store, and just pluck the produt id's so we can compare them later
            List<int> currentInventoryProductId = _locationBL.GetLocationInventory(id).Select(item => item.Product.Id).ToList();
            //and initialize the new inventoryVM instance
            InventoryVM newInventory = new InventoryVM();
            newInventory.LocationId = id;

            //And then initialize the product options
            newInventory.ProductOptions = new List<SelectListItem>();

            //then, grab all products so we can fill out the select by looping over the products and inserting it to the product options list
            List<Product> allProducts = _productBL.GetAllProducts();
            foreach (Product prod in allProducts)
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
            _logger.LogInformation("POST: LocationController/AddInventory/id", inventoryVM);
            try
            {
                if (ModelState.IsValid)
                {
                    _locationBL.AddInventory(new Inventory
                    {
                        ProductId = inventoryVM.ProductId,
                        LocationId = id,
                        Quantity = inventoryVM.Quantity
                    });
                    _logger.LogInformation("Add Inventory Success", inventoryVM);
                    return RedirectToAction(nameof(Inventory), new { id = inventoryVM.LocationId });
                }
                return AddInventory(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Adding inventory failed", inventoryVM);
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
                    _locationBL.UpdateInventoryItem(new Inventory
                    {
                        Id = inventoryVM.Id,
                        ProductId = inventoryVM.ProductId,
                        LocationId = inventoryVM.LocationId,
                        Quantity = inventoryVM.Quantity
                    });
                    return RedirectToAction(nameof(Inventory), new { id = inventoryVM.LocationId });
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
        public async Task<ActionResult> OrderHistory(int id, string sortOrder)
        {
            //set sort order. Default, date ascending
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            Location currentLocation = _locationBL.FindLocationByID(id);
            ViewBag.CurrentLocation = currentLocation;
            //grab the orders
            List<Order> orders = _orderBL.GetOrdersByLocationId(id);
            List<OrderVM> orderVMs = new List<OrderVM>();
            foreach (Order ord in orders)
            {
                //only display closed orders
                if (!ord.Closed) break;

                //initialize new orderVM and get all other necessary infos
                OrderVM ordVM = new OrderVM(ord);
                ordVM.OrderUser = await _userManager.FindByIdAsync(ord.UserId.ToString());
                ordVM.LineItems = _orderBL.GetLineItemsByOrderId(ord.Id);
                orderVMs.Add(ordVM);
            }

            switch (sortOrder)
            {
                case "Price":
                    orderVMs = orderVMs.OrderBy(s => s.Total).ToList();
                    break;

                case "price_desc":
                    orderVMs = orderVMs.OrderByDescending(s => s.Total).ToList();
                    break;

                case "date_desc":
                    orderVMs = orderVMs.OrderByDescending(s => s.DateCreated).ToList();
                    break;

                default:
                    orderVMs = orderVMs.OrderBy(s => s.DateCreated).ToList();
                    break;
            }
            return View(orderVMs);
        }
    }
}