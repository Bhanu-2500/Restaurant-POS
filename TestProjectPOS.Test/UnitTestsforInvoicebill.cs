using FluentAssertions;
using Restaurant_POS.Models;
using Xunit;

namespace TestProjectPOS.Test
{
    public class UnitTestsforInvoicebill
    {
        [Fact]
        public void When_InvoicebillCreated_without_order_and_User()
        {
            //Arrange
            InvoiceBill invoiceBill = new InvoiceBill();

            //Act

            //Assert
            invoiceBill.OrderList.Should().BeNull();
            invoiceBill.User.Should().BeNull();
            invoiceBill.DateTime.Should().Be(DateTime.MinValue);
            invoiceBill.SubTotal.Should().Be(0);
            invoiceBill.TotalDiscount.Should().Be(0);
            invoiceBill.Cash.Should().Be(0);
            invoiceBill.Balance.Should().Be(0);

        }
        [Fact]
        public void When_Created_with_Some_Orders()
        {
            //Arrange
            Product product1 = new Product()
            {
                MentionedPrice = 1000,
                Discount = 10,
            };
            Product product2 = new Product()
            {
                MentionedPrice = 500,
                Discount = 0,
            };
            Product product3 = new Product()
            {
                MentionedPrice = 500,
                Discount = 2,
            };

            User user = new User();

            Order order1 = new Order()
            {
                Id = 1,
                Amount = 10,
                Product = product1,//9000 dis =1000
            };
            Order order2 = new Order()
            {
                Id = 2,
                Amount = 10,
                Product = product2,//5000 dis = 0
            };
            Order order3 = new Order()
            {
                Id = 3,
                Amount = 10,
                Product = product3,//4900 dis =100
            };

            InvoiceBill invoiceBill = new InvoiceBill()
            {
                InvoiceId = 1,
                DateTime = new DateTime(2000,12,09,12,12,12),
                User = user,
                Cash = 20000
            };

            //Act
            List<Order> orders = new List<Order>() { order1, order2, order3 };
            invoiceBill.OrderList = orders;

            //Assert
            invoiceBill.DateTime.Should().Be(new DateTime(2000,12,09,12,12,12));
            invoiceBill.User.Should().NotBeNull();
            invoiceBill.User.Should().Be(user);
            invoiceBill.OrderList.Should().NotBeNull();
            invoiceBill.OrderList.Should().HaveCount(3);
            invoiceBill.OrderList.Should().Contain(order1);
            invoiceBill.OrderList.Should().Contain(order2);
            invoiceBill.OrderList.Should().Contain(order3);
            invoiceBill.SubTotal.Should().Be(18900);
            invoiceBill.TotalDiscount.Should().Be(1100);



        }
    }
}
