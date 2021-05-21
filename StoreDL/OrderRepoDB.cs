using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class OrderRepoDB
    {
        private WssDBContext _context;
        public OrderRepoDB(WssDBContext context)
        {
            _context = context;
        }

        public List<Order> GetOrdersByCustomerAndLocation(int customerId, int locationId) {
            return _context.Orders
            .AsNoTracking()
            .Where(order => order.CustomerId == customerId && order.LocationId == locationId)
            .Select(
                order => order
            )
            .ToList();
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            return _context.Orders
            .AsNoTracking()
            .Where(order => order.CustomerId == customerId)
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

        public LineItem CreateLineItem(LineItem item)
        {
            _context.LineItems.Add(item);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return item;
        }

        public Order GetOpenOrder(int customerId, int locationId)
        {
            Order found = _context.Orders
                .AsNoTracking()
                .FirstOrDefault(order => order.CustomerId == customerId && order.LocationId == locationId && order.Closed == false);
            return found;
        }

        public Order GetOrderById(int orderId)
        {
            Order found = _context.Orders
                .AsNoTracking()
                .FirstOrDefault(order => order.Id == orderId);
            return found;
        }
/// <summary>
/// This method is for adding a new product to an open order
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
        public Order AddItemToOrder(LineItem item)
        {
            _context.LineItems.Add(item);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return GetOrderById(item.OrderId);
        }

/// <summary>
/// this is for changing quantities of an item already in the cart
/// </summary>
/// <param name="item"></param>
/// <returns></returns>
        public Order UpdateItemToOrder(LineItem item)
        {
            LineItem toUpdate = _context.LineItems
            .FirstOrDefault(it => it.Id == item.Id);
            toUpdate.Quantity = item.Quantity;

            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return GetOrderById(item.OrderId);
        }
        public Order CreateOrder(Order order)
        {
            Order added = _context.Orders
                .Add(order).Entity;
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            return added;
            
        }
    }
}