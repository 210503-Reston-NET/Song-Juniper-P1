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
        public List<Order> GetOrdersByCustomerAndLocation(Guid userId, int locationId);

        public List<Order> GetOrdersByCustomerId(Guid userId);
        public List<Order> GetOrdersByLocationId(int locationId);
        public List<LineItem> GetLineItemsByOrderId(int orderId);
        public LineItem CreateLineItem(LineItem item);
        public Order GetOpenOrder(Guid userId, int locationId);
        public Order GetOrderById(int orderId);
        public Order CreateOrder(Order order);
        public LineItem UpdateLineItem(LineItem item);
    }
}
