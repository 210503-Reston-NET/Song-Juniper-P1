using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreModels;

namespace StoreBL
{
    public interface IProductBL
    {
        public Product AddNewProduct(Product product);
        public List<Product> GetAllProducts();
        public Product FindProductByName(string name);
        public Product FindProductById(int id);

    }
}
