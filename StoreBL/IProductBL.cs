using System.Collections.Generic;
using StoreModels;

namespace StoreBL
{
    public interface IProductBL
    {
        public Product AddNewProduct(Product product);

        public List<Product> GetAllProducts();

        public Product FindProductByName(string name);

        public Product FindProductById(int id);

        public Product UpdateProduct(Product product);

        public void DeleteProduct(Product product);
    }
}