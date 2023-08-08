using MyBlog.Models.Login;
using MyBlog.Models.Services;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Models.Services
{
    public class RegistrationDataServiceTests
    {
        [Fact]
        public void GetUserMasterByEmail_ShouldReturnNull_WhenNoMatchingUser()
        {
            // Arrange
            var context = new ApplicationDbContext();
            var service = new RegistrationDataService(context);

            // Act
            var result = service.GetUserMasterByEmail("nonexistent@example.com");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserMasterByEmail_ShouldReturnUser_WhenMatchingUserExists()
        {
            // Arrange
            var context = new ApplicationDbContext();
            var service = new RegistrationDataService(context);
            var userMaster = new UserMaster
            {
                Email = "test@example.com",
                Gender = "Male",
                Mobile = "9898989898",
                Age = 25,
                Name = "Shree Kolekar",
                Password = "Test"
            };
            context.UserMasters.Add(userMaster);
            context.SaveChanges();

            // Act
            var result = service.GetUserMasterByEmail("test@example.com");

            // Assert
            Assert.Equal(userMaster, result);
        }

        [Fact]
        public void PostUserMaster_ShouldAddUserToContext()
        {
            // Arrange
            var context = new ApplicationDbContext();
            var service = new RegistrationDataService(context);
            var userMaster = new UserMaster
            {
                Email = "test@example.com",
                Gender = "Male",
                Mobile = "9898989898",
                Age = 25,
                Name = "Shree Kolekar",
                Password = "Test"
            };

            // Act
            service.PostUserMaster(userMaster);

            // Assert
            Assert.Contains(userMaster, context.UserMasters);
        }
    }
}
