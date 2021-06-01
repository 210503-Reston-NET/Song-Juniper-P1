using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoreModels;
using StoreDL;
using Xunit;

namespace StoreTests
{
    public class ProductRepoTest
    {
        private readonly DbContextOptions<WssDBContext> options;
        public ProductRepoTest()
        {
            options = new DbContextOptionsBuilder<WssDBContext>().UseSqlite("Filename=Test.db").Options;
            Seed();
        }

        [Fact]
        public void GetAllProductsShouldReturnAllProducts()
        {
            using (var context = new WssDBContext(options))
            {
                //Arrange
                IProductRepo _repo = new ProductRepoDB(context);

                //Act
                var products = _repo.GetAllProducts();

                //Assert
                Assert.Equal(3, products.Count);
            }
        }

        [Fact]
        public void GetProductByIdShouldReturnAProduct()
        {
            using (var context = new WssDBContext(options))
            {
                //Arrange
                IProductRepo _repo = new ProductRepoDB(context);
                //Act
                var product = _repo.GetProductById(1);
                //Assert
                Assert.Equal("Banneton", product.Name);
            }
        }

        [Fact]
        public void GetProductByNameShouldReturnAProduct()
        {
            using (var context = new WssDBContext(options))
            {
                //Arrange
                IProductRepo _repo = new ProductRepoDB(context);
                //Act
                var product = _repo.GetProductByName("Banneton");
                //Assert
                Assert.Equal(1, product.Id);
            }
        }

        [Fact]
        public void AddProductShouldAddProduct()
        {
            using (var context = new WssDBContext(options))
            {
                //Arrange
                IProductRepo _repo = new ProductRepoDB(context);
                //Act
                Product product = new Product("Flour", "very good flour", 4.99m, "supply");
                _repo.AddNewProduct(product);
            }
            using (var assertContext = new WssDBContext(options))
            {
                //Assert with different context
                var result = assertContext.Products.FirstOrDefault(product => product.Id == 4);
                Assert.NotNull(result);
                Assert.Equal("Flour", result.Name);
                Assert.Equal("very good flour", result.Description);
                Assert.Equal(4.99m, result.Price);
            }
        }

        [Fact]
        public void UpdateProductShouldUpdateProduct()
        {
            using (var context = new WssDBContext(options))
            {
                //Arrange
                IProductRepo _repo = new ProductRepoDB(context);
                //Act
                Product product = new("Banneton", "Banneton Description", 4.99m, "Tools");
                product.Id = 1;
                _repo.UpdateProduct(product);
            }
            using (var assertContext = new WssDBContext(options))
            {
                //Assert with different context
                var result = assertContext.Products.FirstOrDefault(product => product.Id == 1);
                Assert.NotNull(result);
                Assert.Equal(4.99m, result.Price);
            }
        }

        [Fact]
        public void DeleteProductShouldDeleteProduct()
        {
            using (var context = new WssDBContext(options))
            {
                //Arrange
                IProductRepo _repo = new ProductRepoDB(context);
                //Act
                Product product = new("Banneton", "Banneton Description", 13.99m, "Tools");
                product.Id = 1;
                _repo.DeleteProduct(product);
            }
            using (var assertContext = new WssDBContext(options))
            {
                //Assert with different context
                var result = assertContext.Products.FirstOrDefault(product => product.Id == 1);
                Assert.Null(result);
            }
        }

        private void Seed()
        {
            //this is an exmaple of a using block
            using (var context = new WssDBContext(options))
            {
                //This makes sure that the state of the db gets recreated everytime to maintain modularity of tests
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Products.AddRange
                (
                    new Product
                    {
                        Id = 1,
                        Name = "Banneton",
                        Description = "Banneton Description",
                        Price = 13.99m,
                        Category = "Tools"
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Dough Whisk",
                        Description = "Whisks dough. Very good.",
                        Price = 5.99m,
                        Category = "Tools"
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Lame",
                        Description = "Cuts dough. Very sharp.",
                        Price = 9.99m,
                        Category = "Tools"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
