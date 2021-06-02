using System;

namespace StoreModels
{
    /// <summary>
    /// This data structure models a product and its quantity. The quantity was separated from the product as it could vary from orders and locations.
    /// </summary>
    public class Inventory
    {
        public Inventory()
        {
        }

        public Inventory(Product product, int storeId, int quant)
        {
            this.Product = product;
            this.LocationId = storeId;
            this.Quantity = quant;
        }

        public Inventory(Product product, int storeId, int quant, int id) : this(product, storeId, quant)
        {
            this.Id = id;
        }

        private int _quantity;

        public int Id { get; set; }
        public Product Product { get; set; }

        public int ProductId { get; set; }

        public int LocationId { get; set; }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value < 0)
                {
                    throw new InvalidOperationException("Quantities cannot be negative");
                }
                _quantity = value;
            }
        }

        public override string ToString()
        {
            return $"Name: {this.Product.Name} \nDescription: {this.Product.Description} \nQuantity: {this.Quantity}";
        }
    }
}