using System;
using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    /// <summary>
    /// Business logic class for all things related to orders and ordering process
    /// </summary>
    public class OrderBL : IOrderBL
    {
        private IOrderRepo _repo;

        public OrderBL(IOrderRepo repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// calls repo method for finding one order that has the status open, ie. the order has not yet been placed
        /// </summary>
        /// <param name="customerId">int, customer id that is associated to this order</param>
        /// <param name="locationId">int, location id that is associated to this order</param>
        /// <returns>order, if found</returns>
        public Order GetOpenOrder(Guid userId, int locationId)
        {
            return _repo.GetOpenOrder(userId, locationId);
        }
        /// <summary>
        /// calls the repo method for creating an order
        /// </summary>
        /// <param name="order">order object</param>
        /// <returns>created order</returns>
        public Order CreateOrder (Order order)
        {
            return _repo.CreateOrder(order);
        }
        /// <summary>
        /// Find an order by order id
        /// </summary>
        /// <param name="id">order id for look up</param>
        /// <returns>found order</returns>
        public Order GetOrderById(int id)
        {
            return _repo.GetOrderById(id);
        }
        /// <summary>
        /// Getting order by customer and location id
        /// </summary>
        /// <param name="customerId">Customer.Id</param>
        /// <param name="locationId">Location.Id</param>
        /// <returns>list of all orders under one customer at a particular location</returns>
        public List<Order> GetOrdersByCustomerAndLocation (Guid userId, int locationId)
        {
            return _repo.GetOrdersByCustomerAndLocation(userId, locationId);
        }

        /// <summary>
        /// finds all orders with a particular customer id
        /// </summary>
        /// <param name="customerId">Customer.Id</param>
        /// <returns>list of orders</returns>
        public List<Order> GetOrdersByCustomerId (Guid userId)
        {
            return _repo.GetOrdersByCustomerId(userId);
        }
        /// <summary>
        /// Gets all line items associated to a particular order
        /// </summary>
        /// <param name="orderId">int, Order.Id</param>
        /// <returns>list of all lineitmes attached to that order</returns>
        public List<LineItem> GetLineItemsByOrderId(int orderId)
        {
            return _repo.GetLineItemsByOrderId(orderId);
        }
        /// <summary>
        /// Adds a new lineitem to the order
        /// </summary>
        /// <param name="item"></param>
        /// <returns>added lineitem</returns>
        public LineItem CreateLineItem(LineItem item)
        {
            return _repo.CreateLineItem(item);
        }
        /// <summary>
        /// Gets all orders associated to a particular location
        /// </summary>
        /// <param name="locationId">int, Location.Id</param>
        /// <returns>list of orders</returns>
        public List<Order> GetOrdersByLocationId(int locationId)
        {
            return _repo.GetOrdersByLocationId(locationId);
        }

        /// <summary>
        /// Updates the already existing line item
        /// </summary>
        /// <param name="item">line item to be updated</param>
        /// <returns>updated line item</returns>
        public LineItem UpdateLineItem(LineItem item)
        {
            return _repo.UpdateLineItem(item);
        }
    }
}