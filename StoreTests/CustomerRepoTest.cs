using Microsoft.EntityFrameworkCore;
using StoreModels;
using Xunit;
using StoreDL;
using System.Linq;

namespace StoreTests
{

    /// <summary>
    /// This is going to be my test class for the data access methods in my repo.
    /// </summary>
    public class CustomerRepoTest
    {
        private readonly DbContextOptions<WssDBContext> options;
        //XUnit creates new instances of test classes, so you need to make sure that your db is seeded for each test class

        public CustomerRepoTest()
        {
            options = new DbContextOptionsBuilder<WssDBContext>()
            .UseSqlite("Filename = Test.db")
            .Options;
            Seed();
        }

        //testing read operation
        [Fact]
        public void GetAllCustomersShouldReturnAllCustomers()
        {
            using(var context = new WssDBContext(options))
            {
                //Arrange the test context
                CustomerRepoDB _repo = new CustomerRepoDB(context);

                //Act
                var customers = _repo.GetAllCustomers();
                //Assert
                Assert.Equal(3, customers.Count);
            }
        }

        [Fact]
        public void AddCustomerShouldAddCustomer()
        {
            using (var context = new WssDBContext(options))
            {
                CustomerRepoDB _repo = new CustomerRepoDB(context);
                //Act with a test context
                _repo.AddNewCustomer
                (
                    new Customer("Test User", "test@gmail.com")
                );
            }
            //use a diff context to check if changes persist to db
            using (var assertContext = new WssDBContext(options))
            {
                //Assert with a different context
                var result = assertContext.Customers.FirstOrDefault(cust => cust.Id == 4);
                Assert.NotNull(result);
                Assert.Equal("Test User", result.Name);
            }
        }

        private void Seed()
        {
            //Seed our DB with this method
            //example of using block
            using (var context = new WssDBContext(options))
            {
                //this makes sure that the state of the db gets recreated each time to maintain the modularity of the test
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                context.Customers.AddRange
                (
                    new Customer {
                        Id = 1,
                        Name = "Auryn",
                        Email = "auryn@gmail.com"
                    },
                    new Customer {
                        Id = 2,
                        Name = "Melix",
                        Email = "melix@gmail.com"
                    },
                    new Customer {
                        Id = 3,
                        Name = "Roaringsheep",
                        Email = "roaringsheep@gmail.com"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}