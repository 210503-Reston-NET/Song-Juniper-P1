using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;

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
            
            //then, get the order with closed: false property associated to the current user's id and the location's id
            Order openOrder = _orderBL.GetOpenOrder(new Guid(currentUserId), id);
            //if there is no such order, create a new one and persist it to the db
            if (openOrder is null)
            {
                Order order = new Order();
                order.LocationId = id;
                order.UserId = new Guid(currentUserId);
                order.Closed = false;
                openOrder = _orderBL.CreateOrder(order);
            }
            //Also get all lineItems associated to this order
            openOrder.LineItems = _orderBL.GetLineItemsByOrderId(openOrder.Id) ?? new List<LineItem>();

            //finally, return the view with order.
            return View(openOrder);
        }

        // GET: CartController/Create
        public ActionResult Create()
        {
            return View();
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
