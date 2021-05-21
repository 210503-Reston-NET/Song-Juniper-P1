using System;
using Xunit;
using StoreModels;

namespace StoreTests
{
    public class InventoryTest
    {
        [Fact]
        public void QuantityShouldNotBeNegative()
        {
            Inventory test = new Inventory();
            Assert.Throws<Exception>(() => test.Quantity = -1);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(0)]
        [InlineData(304)]
        public void InventoryShouldSetValidQuantity(int input)
        {
            Inventory test = new Inventory();
            test.Quantity = input;
            Assert.Equal(test.Quantity, input);
        }

        [Fact]
        public void InventoryShouldSetValidProduct()
        {
            Inventory test = new Inventory();
            Product product = new Product();
            test.Product = product;
            Assert.Equal(test.Product, product);
        }
    }
}