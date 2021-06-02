using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderBL _orderBL;
        private readonly ILocationBL _locationBL;
        private readonly ILogger<OrderController> _logger;

        public OrderController(UserManager<User> userManager, IOrderBL orderBL, ILocationBL locationBL, ILogger<OrderController> logger)
        {
            _userManager = userManager;
            _orderBL = orderBL;
            _locationBL = locationBL;
            _logger = logger;
        }

        public Order GetOpenOrder(int locationId, Guid userId)
        {
            _logger.LogInformation("Getting the open order: LocationId, UserId: ", locationId, userId);
            //get the order with closed: false property associated to the current user's id and the location's id
            Order openOrder = _orderBL.GetOpenOrder(userId, locationId);
            //if there is no such order, create a new one and persist it to the db
            if (openOrder is null)
            {
                Order order = new Order();
                order.LocationId = locationId;
                order.UserId = userId;
                order.Closed = false;
                openOrder = _orderBL.CreateOrder(order);
            }
            //Also get all lineItems associated to this order
            openOrder.LineItems = _orderBL.GetLineItemsByOrderId(openOrder.Id) ?? new List<LineItem>();
            //And calculate total
            openOrder.UpdateTotal();
            return openOrder;
        }

        // GET: CartController/5
        /// <summary>
        /// Shows the current "Open" order object associated to this location and the current customer
        /// </summary>
        /// <param name="id">location Id</param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            _logger.LogInformation("Getting the cart view... Location Id: ", id);
            //first, get the current user's Guid
            string currentUserId = _userManager.GetUserId(User);

            //and the information about the current Store they're shopping at
            Location currentLocation = _locationBL.FindLocationByID(id);
            //and store it in the viewbag
            ViewBag.CurrentLocation = currentLocation;

            //And get the open order for this store and the current user
            Order openOrder = GetOpenOrder(id, new Guid(currentUserId));
            //finally, return the view with order.
            return View(openOrder);
        }

        /// <summary>
        /// Places order
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <param name="order">Order Object to be "placed"</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(int id, Order order)
        {
            //change the dateCreated to now and change the status to closed
            order.DateCreated = DateTime.Now;
            order.Closed = true;

            //fetch the lineItems and attach them to the order
            List<LineItem> orderLineItems = _orderBL.GetLineItemsByOrderId(order.Id);
            order.LineItems = orderLineItems;

            try
            {
                _logger.LogInformation("Placing an order... ", order);
                //place it!
                order = _orderBL.UpdateOrder(order);
                //get store inventory so we can subtract the sold items
                List<Inventory> locationInventory = _locationBL.GetLocationInventory(order.LocationId);
                //loop through each line item, find it in the inventory, and update the inventory quantity
                foreach (LineItem lineItem in orderLineItems)
                {
                    Inventory toModify = locationInventory.Find(inventory => inventory.ProductId == lineItem.ProductId);
                    if (toModify is not null)
                    {
                        toModify.Quantity -= lineItem.Quantity;
                        try
                        {
                            _logger.LogInformation("Updating inventory items", toModify);
                            _locationBL.UpdateInventoryItem(toModify);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to update inventory quantity after the order");
                        }
                    }
                }
                _logger.LogInformation("Order successfully placed!", order);
                //tada!
                return RedirectToAction(nameof(Index), "Location");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Placing order failed");
            }
            return Index(id);
        }

        /// <summary>
        /// GET: LocationController/AddToCart/5
        /// Display Page to add products to the customer's cart
        /// </summary>
        /// <param name="id">inventory Id</param>
        /// <returns></returns>
        public ActionResult AddToCart(int id)
        {
            //first of all, get the inventory item to add to the order
            Inventory inventoryItem = _locationBL.GetInventoryById(id);
            //And store the location data, so users can go back.
            ViewBag.LocationId = inventoryItem.LocationId;

            //and get the current user Id
            string currentUserId = _userManager.GetUserId(User);
            //And get the open order
            Order openOrder = GetOpenOrder(inventoryItem.LocationId, new Guid(currentUserId));

            //and see if the item we're trying to add is already in the "cart"
            LineItem item = openOrder.LineItems.Find(item => item.Product.Id == inventoryItem.ProductId);
            if (item is null)
            {
                //if we didn't find it, then initialize it from the inventory item.
                item = new LineItem();
                item.Product = inventoryItem.Product;
                item.ProductId = inventoryItem.ProductId;
                item.OrderId = openOrder.Id;
                item.Quantity = 0;
            }
            return View(item);
        }

        /// <summary>
        /// POST: LocationController/AddToCart/5
        /// persists the form data to db
        /// </summary>
        /// <param name="inventoryId">inventory ID</param>
        /// <param name="lineItem">form data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int inventoryId, LineItem lineItem)
        {
            _logger.LogInformation("Adding line item to cart");
            try
            {
                if (ModelState.IsValid)
                {
                    if (lineItem.Id != 0)
                    {
                        _logger.LogInformation("Item already exists in the order. Updating the quantity", lineItem);
                        //this item is already in the order, just update the quantity
                        _orderBL.UpdateLineItem(new LineItem
                        {
                            Id = lineItem.Id,
                            ProductId = lineItem.ProductId,
                            OrderId = lineItem.OrderId,
                            Quantity = lineItem.Quantity
                        });
                    }
                    else
                    {
                        _logger.LogInformation("Creating new line item for this order", lineItem);
                        _orderBL.CreateLineItem(new LineItem {
                            Id = lineItem.Id,
                            ProductId = lineItem.ProductId,
                            OrderId = lineItem.OrderId,
                            Quantity = lineItem.Quantity
                        });
                    }
                    return RedirectToAction(nameof(Inventory), "Location", new { id = _orderBL.GetOrderById(lineItem.OrderId).LocationId });
                }
                return AddToCart(inventoryId);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to add lineitem to the cart");
                return AddToCart(inventoryId);
            }
        }

        public async Task<ActionResult> UserOrderHistory(string sortOrder)
        {
            //set sort order. Default, date ascending
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            User currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.currentUser = currentUser;

            //grab the orders
            List<Order> orders = _orderBL.GetOrdersByCustomerId(currentUser.Id);
            List<OrderVM> orderVMs = new List<OrderVM>();
            foreach (Order ord in orders)
            {
                //only display closed orders
                if (!ord.Closed) break;

                //initialize new orderVM and get all other necessary infos
                OrderVM ordVM = new OrderVM(ord);
                ordVM.OrderLocation = _locationBL.FindLocationByID(ord.LocationId);
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