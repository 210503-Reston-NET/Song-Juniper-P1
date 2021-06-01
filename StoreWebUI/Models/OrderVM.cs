using System;
using System.Collections.Generic;
using StoreModels;

namespace StoreWebUI.Models
{
    public class OrderVM
    {
        public OrderVM()
        {
        }

        public OrderVM(Order order)
        {
            Id = order.Id;
            DateCreated = order.DateCreated;
            UserId = order.UserId;
            LocationId = order.LocationId;
            Closed = order.Closed;
            Total = order.Total;
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid UserId { get; set; }
        public int LocationId { get; set; }
        public bool Closed { get; set; }
        public decimal Total { get; set; }
        public User OrderUser { get; set; }
        public Location OrderLocation { get; set; }
        public List<LineItem> LineItems { get; set; }
    }
}