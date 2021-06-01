using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StoreBL;
using StoreModels;
using StoreWebUI.Controllers;
using StoreWebUI.Models;
using Xunit;

namespace StoreTests
{
    public class ControllerTest
    {
        [Fact]
        public void LocationControllerIndexShouldReturnLocations()
        {
            //Arrange
            var mockLocationBL = new Mock<ILocationBL>();
            mockLocationBL.Setup(x => x.GetAllLocations()).Returns(
                new List<Location>()
                {
                    new Location("locOne", "addressOne"),
                    new Location("locTwo", "addressTwo")
                }
            );
            var mockProductBL = new Mock<IProductBL>();
            var mockOrderBL = new Mock<IOrderBL>();
            var mockUserManager = TestUserManager<User>();
            var controller = new LocationController(mockLocationBL.Object, mockProductBL.Object, mockOrderBL.Object, mockUserManager);
            //Act
            var result = controller.Index();
            //Assert
            //Check that we're getting a view as a result
            var viewResult = Assert.IsType<ViewResult>(result);
            //Check that the model of the viewResult is a list of restaurant VMs
            var model = Assert.IsAssignableFrom<IEnumerable<LocationVM>>(viewResult.ViewData.Model);
            //Check that we're getting the same amount of restaurants that we're returning
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void OrderControllerGetOpenOrderShouldReturnOpenOrder()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            int locationId = 1;
            DateTime now = DateTime.Now;
            Order mockOrder = new Order();
            var mockOrderBL = new Mock<IOrderBL>();
            mockOrderBL.Setup(x => x.CreateOrder(mockOrder)).Returns(
                new Order
                {
                    Id = 1,
                    LocationId = locationId,
                    UserId = userId,
                    Closed = false,
                    DateCreated = now
                }
            );
            mockOrderBL.Setup(x => x.GetOpenOrder(userId, locationId)).Returns
            (
                new Order
                {
                    Id = 1,
                    LocationId = locationId,
                    UserId = userId,
                    Closed = false,
                    DateCreated = now
                }
            );
            mockOrderBL.Setup(x => x.GetLineItemsByOrderId(1)).Returns
            (
                new List<LineItem>()
                {
                    new LineItem
                    {
                        Product = new Product("one", "desc", 3m, "cat"),
                        OrderId = 1,
                        ProductId = 1,
                        Quantity = 3
                    },
                    new LineItem
                    {
                        Product = new Product("one", "desc", 3m, "cat"),
                        OrderId = 1,
                        ProductId = 1,
                        Quantity = 3
                    }
                }
            );
            var mockLocationBL = new Mock<ILocationBL>();
            var mockUserManager = TestUserManager<User>();
            var controller = new OrderController(mockUserManager, mockOrderBL.Object, mockLocationBL.Object);

            //Act
            var result = controller.GetOpenOrder(locationId, userId);

            //Assert
            //Are we getting an Order type object?
            Assert.IsType<Order>(result);
            //make sure the order is the order we want
            Assert.False(result.Closed);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(locationId, result.LocationId);
            Assert.Equal(now, result.DateCreated);
            Assert.Equal(18m, result.Total);
        }

        public static UserManager<TUser> TestUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            var options = new Mock<IOptions<IdentityOptions>>();
            var idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            var userValidators = new List<IUserValidator<TUser>>();
            var validator = new Mock<IUserValidator<TUser>>();
            userValidators.Add(validator.Object);
            var pwdValidators = new List<PasswordValidator<TUser>>();
            pwdValidators.Add(new PasswordValidator<TUser>());
            var userManager = new UserManager<TUser>(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }
    }
}