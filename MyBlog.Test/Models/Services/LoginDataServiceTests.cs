using Microsoft.EntityFrameworkCore;
using Moq;
using MyBlog.Models.Login;
using MyBlog.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Models.Services
{
    public class LoginDataServiceTests
    {
        [Fact]
        public void AuthenticateUser_ShouldReturnNull_WhenNoMatchingUser()
        {
            // Arrange
            var userMasters = new List<UserMaster>();
            var userMasterMock = new Mock<DbSet<UserMaster>>();
            userMasterMock.As<IQueryable<UserMaster>>().Setup(m => m.Provider).Returns(userMasters.AsQueryable().Provider);
            userMasterMock.As<IQueryable<UserMaster>>().Setup(m => m.Expression).Returns(userMasters.AsQueryable().Expression);
            userMasterMock.As<IQueryable<UserMaster>>().Setup(m => m.ElementType).Returns(userMasters.AsQueryable().ElementType);
            userMasterMock.As<IQueryable<UserMaster>>().Setup(m => m.GetEnumerator()).Returns(userMasters.AsQueryable().GetEnumerator());

            var contextMock = new Mock<IApplicationDbContext>();
            contextMock.Setup(c => c.UserMasters).Returns(userMasterMock.Object);

            var service = new LoginDataService(contextMock.Object);
            var login = new User { Username = "nonexistent@example.com", Password = "password" };

            // Act
            var result = service.AuthenticateUser(login);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void AuthenticateUser_ShouldReturnUser_WhenMatchingUserExists()
        {
            // Arrange
            var userMaster = new UserMaster { Email = "test@example.com", Password = "password" };
            var userMasters = new List<UserMaster> { userMaster };
            var userMasterMock = new Mock<DbSet<UserMaster>>();
            userMasterMock.As<IQueryable<UserMaster>>().Setup(m => m.Provider).Returns(userMasters.AsQueryable().Provider);
            userMasterMock.As<IQueryable<UserMaster>>().Setup(m => m.Expression).Returns(userMasters.AsQueryable().Expression);
            userMasterMock.As<IQueryable<UserMaster>>().Setup(m => m.ElementType).Returns(userMasters.AsQueryable().ElementType);
            userMasterMock.As<IQueryable<UserMaster>>().Setup(m => m.GetEnumerator()).Returns(userMasters.AsQueryable().GetEnumerator());

            var contextMock = new Mock<IApplicationDbContext>();
            contextMock.Setup(c => c.UserMasters).Returns(userMasterMock.Object);

            var service = new LoginDataService(contextMock.Object);
            var login = new User { Username = "test@example.com", Password = "password" };

            // Act
            var result = service.AuthenticateUser(login);

            // Assert
            Assert.Equal(userMaster, result);
        }
    }
}
