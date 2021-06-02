using System;
using System.Collections.Generic;
using System.Text;

namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a customer order.
    /// </summary>
    public class Order
    {
        public Order()
        {
            this.DateCreated = DateTime.Now;
            this.Closed = false;
        }

        public Order(Guid userId, int storeId) : this()
        {
            this.UserId = userId;
            this.LocationId = storeId;
        }

        public Order(Guid userId, int storeId, int id) : this(userId, storeId)
        {
            this.Id = id;
        }

        public Order(Guid userId, int storeId, List<LineItem> items) : this(userId, storeId)
        {
            this.LineItems = items;
        }

        public Order(Guid userId, int storeId, int id, List<LineItem> items) : this(userId, storeId, id)
        {
            this.LineItems = items;
        }

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public Guid UserId { get; set; }
        public int LocationId { get; set; }
        public List<LineItem> LineItems { get; set; }

        public bool Closed { get; set; }
        public decimal Total { get; set; }

        public override string ToString()
        {
            StringBuilder ItemString = new StringBuilder();
            foreach (LineItem item in this.LineItems)
            {
                ItemString.Append('\n').Append(item.ToString());
            }
            return $"Date Created: {this.DateCreated.ToString("D")} \nItems: {ItemString.ToString()} \nTotal: {this.Total}";
        }

        public void UpdateTotal()
        {
            if (this.LineItems is null) this.Total = new decimal();
            decimal total = new decimal();
            foreach (LineItem item in this.LineItems)
            {
                total += item.Product.Price * item.Quantity;
            }
            this.Total = Math.Round(total, 2);
        }
    }
}