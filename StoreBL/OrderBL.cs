using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    public class OrderBL : IOrderBL
    {
        private IOrderRepo _repo;

        public OrderBL(IOrderRepo repo)
        {
            _repo = repo;
        }

        public Order GetOpenOrder(int customerId, int locationId)
        {
            return _repo.GetOpenOrder(customerId, locationId);
        }

        public Order CreateOrder (Order order)
        {
            return _repo.CreateOrder(order);
        }

        public List<Order> GetOrdersByCustomerAndLocation (int customerId, int locationId)
        {
            return _repo.GetOrdersByCustomerAndLocation(customerId, locationId);
        }

        public List<Order> GetOrdersByCustomerId (int customerId)
        {
            return _repo.GetOrdersByCustomerId(customerId);
        }

        public List<LineItem> GetLineItemsByOrderId(int orderId)
        {
            return _repo.GetLineItemsByOrderId(orderId);
        }

        public LineItem CreateLineItem(LineItem item)
        {
            return _repo.CreateLineItem(item);
        }

        public List<Order> GetOrdersByLocationId(int locationId)
        {
            return _repo.GetOrdersByLocationId(locationId);
        }
    }
}