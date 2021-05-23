using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreModels;

namespace StoreDL
{
    public interface IProductRepo
    {
        public List<Product> GetAllProducts();
        public Product GetProductById(int id);
        public Product GetProductByName(string name);
        public Product AddNewProduct(Product product);
    }
}
