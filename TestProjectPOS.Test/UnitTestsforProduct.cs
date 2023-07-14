using FluentAssertions;
using Restaurant_POS.Models;
using Xunit;

namespace TestProjectPOS.Test
{
    public class UnitTestsforProduct
    {
        [Fact]
        public void When_created_Product_MentionedPrice_and_Discount_cannotbe_Negative()
        {
            //Arrange
            Product product = new Product();

            //Act

            //Assert
            product.MentionedPrice.Should().BeGreaterThanOrEqualTo(0);
            product.Discount.Should().BeGreaterThanOrEqualTo(0);
            product.Discount.Should().BeLessThanOrEqualTo(100);
        }
        [Fact]
        public void When_Created_Product_without_Discount()
        {
            //Arrange
            Product product = new Product();

            //Act
            product.MentionedPrice = 100;
            product.Discount = 0;

            //Assert
            product.SellingPrice.Should().Be(100);
            product.DiscountPrice.Should().Be(0);
            product.IsDiscountAvailable.Should().BeFalse();
        }
        [Fact]
        public void When_Created_Product_with_Discount()
        {
            //Arrange
            Product product = new Product();

            //Act
            product.MentionedPrice = 100; 
            product.Discount = 10;

            //Assert
            product.IsDiscountAvailable.Should().BeTrue();
            product.Discount.Should().BeGreaterThan(0);
            product.Discount.Should().Be(10);
            product.SellingPrice.Should().BeLessThan(product.MentionedPrice);
            product.SellingPrice.Should().Be(90);
            product.DiscountPrice.Should().Be(10);
        }
    }
}