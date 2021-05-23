using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StoreModels;

namespace StoreDL
{
    public class ProductRepoDB : IProductRepo
    {
        private WssDBContext _context;
        public ProductRepoDB(WssDBContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products
            .Select(
                prod => prod
            ).ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(product => product.Id == id);
        }

        public Product GetProductByName(string name)
        {
            return _context.Products.FirstOrDefault(product => product.Name == name);
        }

        public Product AddNewProduct(Product product)
        {
            Product prodToAdd = _context.Products.Add(product).Entity;
            _context.SaveChanges();

            return prodToAdd;
        }
    }
}