using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreModels;

namespace StoreDL
{
    public interface IOrderRepo
    {
        public List<Order> GetOrdersByCustomerAndLocation(int customerId, int locationId);

        public List<Order> GetOrdersByCustomerId(int customerId);
        public List<Order> GetOrdersByLocationId(int locationId);
        public List<LineItem> GetLineItemsByOrderId(int orderId);
        public LineItem CreateLineItem(LineItem item);
        public Order GetOpenOrder(int customerId, int locationId);
        public Order GetOrderById(int orderId);

        public Order CreateOrder(Order order);
    }
}
