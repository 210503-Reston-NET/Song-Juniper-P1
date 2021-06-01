using System;

namespace StoreModels
{
    //This class should contain all necessary fields to define a product.
    public class Product
    {
        private string _name;
        private decimal _price;
        public int Id { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Length == 0)
                {
                    throw new Exception("Product Name cannot be empty");
                }
                _name = value;
            }
        }

        public string Description { get; set; }

        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Price cannot be negative");
                }
                _price = value;
            }
        }

        public string Category { get; set; }

        public Product(string name, string desc, decimal price, string cat)
        {
            this.Name = name;
            this.Description = desc;
            this.Price = price;
            this.Category = cat;
        }

        public Product(int id, string name, string desc, decimal price, string cat) : this(name, desc, price, cat)
        {
            this.Id = id;
        }

        public Product(int id)
        {
            this.Id = id;
        }

        public Product()
        {
        }

        public override string ToString()
        {
            return $"Name: {this.Name} \nDescription: {this.Description} \nPrice: {this.Price} \nCategory: {this.Category}";
        }
    }
}