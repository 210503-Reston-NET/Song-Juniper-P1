using System.Collections.Generic;
using StoreModels;

namespace StoreDL
{
    public interface IProductRepo
    {
        public List<Product> GetAllProducts();

        public Product GetProductById(int id);

        public Product GetProductByName(string name);

        public Product AddNewProduct(Product product);

        public Product UpdateProduct(Product product);

        public void DeleteProduct(Product product);
    }
}