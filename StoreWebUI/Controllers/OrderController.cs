using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;

namespace StoreWebUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IOrderBL _orderBL;
        private readonly ILocationBL _locationBL;

        public OrderController(UserManager<User> userManager, IOrderBL orderBL, ILocationBL locationBL)
        {
            _userManager = userManager;
            _orderBL = orderBL;
            _locationBL = locationBL;
        }

        public Order GetOpenOrder(int locationId, Guid userId)
        {
            //then, get the order with closed: false property associated to the current user's id and the location's id
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
            if(item is null)
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
            try
            {
                if (ModelState.IsValid)
                {
                    if(lineItem.Id != 0)
                    {
                        //this item is already in the order, just update the quantity
                        _orderBL.UpdateLineItem(lineItem);
                    }
                    else
                    {
                        _orderBL.CreateLineItem(lineItem);
                    }
                    return RedirectToAction(nameof(Inventory), "Location", new { id = _orderBL.GetOrderById(lineItem.OrderId).LocationId});
                }
                return AddToCart(inventoryId);
            }
            catch
            {
                return AddToCart(inventoryId);
            }
        }

        // POST: CartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CartController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
