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
        public Order GetOpenOrder(int customerId, int locationId);
        public Order CreateOrder(Order order);
        public List<Order> GetOrdersByCustomerAndLocation(int customerId, int locationId);
        public List<Order> GetOrdersByCustomerId(int customerId);
        public List<LineItem> GetLineItemsByOrderId(int orderId);
        public LineItem CreateLineItem(LineItem item);
        public List<Order> GetOrdersByLocationId(int locationId);
    }
}
