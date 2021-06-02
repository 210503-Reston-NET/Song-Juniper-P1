using System;
using StoreModels;
using Xunit;

namespace StoreTests
{
    public class LineItemTest
    {
        [Fact]
        public void QuantityShouldNotBeNegative()
        {
            LineItem test = new LineItem();
            Assert.Throws<InvalidOperationException>(() => test.Quantity = -1);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(0)]
        [InlineData(304)]
        public void LineItemShouldSetValidQuantity(int input)
        {
            LineItem test = new LineItem();
            test.Quantity = input;
            Assert.Equal(test.Quantity, input);
        }

        [Fact]
        public void LineItemShouldSetValidProduct()
        {
            LineItem test = new LineItem();
            Product product = new Product();
            test.Product = product;
            Assert.Equal(test.Product, product);
        }
    }
}