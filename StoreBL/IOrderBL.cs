using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreModels;

namespace StoreBL
{
    public interface IOrderBL
    {
        public Order CreateOrder(Order order);
        public Order UpdateOrder(Order order);
        public Order GetOpenOrder(Guid userId, int locationId);
        public Order GetOrderById(int orderId);
        public List<Order> GetOrdersByCustomerId(Guid userId);
        public List<Order> GetOrdersByLocationId(int locationId);
        public List<Order> GetOrdersByCustomerAndLocation(Guid userId, int locationId);
        public List<LineItem> GetLineItemsByOrderId(int orderId);
        public LineItem CreateLineItem(LineItem item);
        public LineItem UpdateLineItem(LineItem item);
    }
}
