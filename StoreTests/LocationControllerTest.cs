﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using StoreBL;
using StoreModels;
using StoreWebUI.Models;
using StoreWebUI.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace StoreTests
{
    public class LocationControllerTest
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
