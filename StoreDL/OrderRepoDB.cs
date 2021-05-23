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