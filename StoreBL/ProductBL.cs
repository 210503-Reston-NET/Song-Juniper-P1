using System;
using System.Collections.Generic;
using StoreDL;
using StoreModels;

namespace StoreBL
{
    /// <summary>
    /// All business logic related CRUD'ing products
    /// </summary>
    public class ProductBL : IProductBL
    {
        private IProductRepo _repo;
        public ProductBL(IProductRepo repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// create a new product, checks for name duplication before proceeding
        /// </summary>
        /// <param name="prod">product object</param>
        /// <returns>created product</returns>
        public Product AddNewProduct(Product prod)
        {
            if(FindProductByName(prod.Name) is not null)
                {
                    throw new Exception("There is already a product with the same name");
                }
            return _repo.AddNewProduct(prod);
        }
        /// <summary>
        /// gets all product
        /// </summary>
        /// <returns>list of products</returns>
        public List<Product> GetAllProducts()
        {
            return _repo.GetAllProducts();
        }
        /// <summary>
        /// look for a product by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>found product</returns>
        public Product FindProductByName(string name)
        {
            return _repo.GetProductByName(name);
        }
        /// <summary>
        /// look for a product by id
        /// </summary>
        /// <param name="id">int Product.Id</param>
        /// <returns>found product</returns>
        public Product FindProductById(int id)
        {
            return _repo.GetProductById(id);
        }
    }
}
