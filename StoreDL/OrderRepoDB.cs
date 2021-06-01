using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class OrderRepoDB : IOrderRepo
    {
        private WssDBContext _context;
        public OrderRepoDB(WssDBContext context)
        {
            _context = context;
        }

        public List<Order> GetOrdersByCustomerAndLocation(Guid userId, int locationId) {
            return _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId.Equals(userId) && order.LocationId == locationId)
            .Select(
                order => order
            )
            .ToList();
        }

        public List<Order> GetOrdersByCustomerId(Guid userId)
        {
            return _context.Orders
            .AsNoTracking()
            .Where(order => order.UserId.Equals(userId))
            .Select(order => order)
            .ToList();
        }

        public List<Order> GetOrdersByLocationId(int locationId)
        {
            return _context.Orders
            .AsNoTracking()
            .Where(order => order.LocationId == locationId)
            .Select(order => order)
            .ToList();
        }

        public List<LineItem> GetLineItemsByOrderId(int orderId)
        {
            return _context.LineItems
            .AsNoTracking()
            .Include("Product")
            .Where(item => item.OrderId == orderId)
            .Select(item => item)
            .ToList();
        }
        /// <summary>
        /// Creates a new line item object
        /// </summary>
        /// <param name="item">line item to be created</param>
        /// <returns>created line item</returns>
        public LineItem CreateLineItem(LineItem item)
        {
            LineItem added = _context.LineItems.Add(item).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return added;
        }
        /// <summary>
        /// Updates an existing line item object
        /// </summary>
        /// <param name="item">line item object to be updated</param>
        /// <returns>updated line item object</returns>
        public LineItem UpdateLineItem(LineItem item)
        {
            LineItem updated = _context.LineItems.Update(item).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return updated;
        }

        public Order GetOpenOrder(Guid userId, int locationId)
        {
            Order found = _context.Orders
                .AsNoTracking()
                .FirstOrDefault(order => order.UserId.Equals(userId) && order.LocationId == locationId && order.Closed == false);
            return found;
        }

        public Order GetOrderById(int orderId)
        {
            Order found = _context.Orders
                .AsNoTracking()
                .FirstOrDefault(order => order.Id == orderId);
            return found;
        }

        public Order CreateOrder(Order order)
        {
            Order added = _context.Orders
                .Add(order).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return added;
        }

        public Order UpdateOrder(Order order)
        {
            Order updated = _context.Orders.Update(order).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return updated;
        }
    }
}