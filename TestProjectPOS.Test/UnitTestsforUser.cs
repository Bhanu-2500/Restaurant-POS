
using FluentAssertions;
using Restaurant_POS.Models;

namespace TestProjectPOS.Test
{
    public class UnitTestsforUser
    {
        [Fact]
        public void When_Normal_User_Created()
        {
            //Arrange
            User normalUser = new User() 
            {
                Id = 1,
                Name = "Test",
                Password = "Test",
                Email = "Test",
                PhoneNumber = "Test",
                ImagePath = "c:user/newFolder/image.png",
            };

            //Act 
            normalUser.IsAdmin = false;

            //Assert
            normalUser.IsAdmin.Should().BeFalse();
        }
        public void When_Admin_User_Created()
        {
            //Arrange
            User normalUser = new User() 
            {
                Id = 1,
                Name = "Test",
                Password = "Test",
                Email = "Test",
                PhoneNumber = "Test",
                ImagePath = "c:user/newFolder/image.png",
            };

            //Act 
            normalUser.IsAdmin = true;

            //Assert
            normalUser.IsAdmin.Should().BeTrue();
        }
    }
}
