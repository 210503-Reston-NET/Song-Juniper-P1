using System;
using Xunit;
using StoreModels;

namespace StoreTests
{
    public class ProductTest
    {
        [Fact]
        public void NameShouldNotBeEmpty()
        {
            Product test = new Product();
            Assert.Throws<Exception>(() => test.Name = "");
        }

        [Fact]
        public void PriceShouldNotBeNegative()
        {
            Product test = new Product();
            Assert.Throws<Exception>(() => test.Price = -1);
        }

    }
}