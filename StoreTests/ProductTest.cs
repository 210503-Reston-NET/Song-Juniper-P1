using System;
using StoreModels;
using Xunit;

namespace StoreTests
{
    public class ProductTest
    {
        [Fact]
        public void NameShouldNotBeEmpty()
        {
            Product test = new Product();
            Assert.Throws<InvalidOperationException>(() => test.Name = "");
        }

        [Fact]
        public void PriceShouldNotBeNegative()
        {
            Product test = new Product();
            Assert.Throws<InvalidOperationException>(() => test.Price = -1);
        }
    }
}