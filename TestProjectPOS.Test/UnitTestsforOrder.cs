
using FluentAssertions;
using Restaurant_POS.Models;
using Xunit;

namespace TestProjectPOS.Test
{
    public class UnitTestsforOrder
    {
        [Fact]
        public void When_Created_Order_Amount_Cannotbe_Negative()
        {
            //Arrange
            Order order = new Order();

            //Act

            //Assert
            order.Amount.Should().BeGreaterThanOrEqualTo(1);
            order.Product.Should().BeNull();
            order.SubTotal.Should().Be(0);
            order.TotalDiscount.Should().Be(0); 
        }
        [Fact]
        public void When_Created_SubTotal_Equalto_SellingPriceoftheProduct_times_Amount()
        {
            //Arrange
            Product product = new Product() 
            {
                MentionedPrice = 100,
            };
            Order order = new Order()
            {
                Product = product,
                Amount = 10,
            };
            //Act

            //Assert
            order.SubTotal.Should().Be(1000);
            order.TotalDiscount.Should().Be(0);

        }

        [Fact]
        public void When_Order_Created_ProductwithDiscount()
        {
            //Arrange
            Product product = new Product()
            {
                MentionedPrice = 100,
                Discount = 10
            };
            Order order = new Order()
            {
                Product = product,
                Amount = 10
            };
            //Act

            //Assert
            order.TotalDiscount.Should().Be(100);
            order.SubTotal.Should().Be(900);
        }

    }
}
