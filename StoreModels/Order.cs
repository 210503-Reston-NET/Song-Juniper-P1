using System;
using System.Collections.Generic;

namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a customer order. 
    /// </summary>
    public class Order
    {
        public Order() {
            this.DateCreated = DateTime.Now;
            this.Closed = false;
        }

        public Order(int customerId, int storeId) : this()
        {
            this.CustomerId = customerId;
            this.LocationId = storeId;
        }
        public Order(int customerId, int storeId, int id) : this(customerId, storeId)
        {
            this.Id = id;
        }

        public Order(int customerId, int storeId, List<LineItem> items) : this(customerId, storeId)
        {
            this.LineItems = items;
        }

        public Order(int customerId, int storeId, int id, List<LineItem> items) : this(customerId, storeId, id)
        {
            this.LineItems = items;
        }
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public List<LineItem> LineItems { get; set; }

        public bool Closed { get; set; }
        public decimal Total { get; set; }

        public override string ToString()
        {
            string ItemString = "";
            foreach(LineItem item in this.LineItems)
            {   
                ItemString += "\n" + item.ToString();
            }
            return $"Date Created: {this.DateCreated.ToString("D")} \nItems: {ItemString} \nTotal: {this.Total}";
        }

        public void UpdateTotal()
        {
            if(this.LineItems is null) this.Total = new decimal();
            decimal total = new decimal();
            foreach(LineItem item in this.LineItems)
            {
                total += item.Product.Price * item.Quantity;
            }
            this.Total = Math.Round(total, 2);
        }

    }
}