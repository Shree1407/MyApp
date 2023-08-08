using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using MyBlog.Models;
using MyBlog.Models.Blog;
using MyBlog.Models.Login;
using MyBlog.Models.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Controllers
{
    public class LoginControllerTests
    {

        [Fact]
        public void Login_ValidCredentials_ReturnsOkResultWithTokenAndUser()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            var loginDataMock = new Mock<ILoginData>();
            var controller = new LoginController(configMock.Object, loginDataMock.Object);
            var user = new User { Username = "test@example.com", Password = "password" };
            loginDataMock.Setup(x => x.AuthenticateUser(user)).Returns(new UserMaster { Email = "test@example.com", Password = "password", Name = "Test User" });

            // Act
            var result = controller.Login(user) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsType<OkObjectResult>(result); 
        }
        [Fact]
        public void Login_InvalidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            var loginDataMock = new Mock<ILoginData>();
            var controller = new LoginController(configMock.Object, loginDataMock.Object);
            var user = new User { Username = "test@example.com", Password = "password" };
            loginDataMock.Setup(x => x.AuthenticateUser(user)).Returns((UserMaster)null);
            // Act
            var result = controller.Login(user) as UnauthorizedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(401, result.StatusCode);
        }
        [Fact]
        public void GenerateToken_ValidUserId_ReturnsJwtTokenString()
        {
            // Arrange
            var configMock = new Mock<IConfiguration>();
            var loginDataMock = new Mock<ILoginData>();
            var controller = new LoginController(configMock.Object, loginDataMock.Object);
            var userId = "test@example.com";

            // Act
            var methodInfo = typeof(LoginController).GetMethod("GenerateToken", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = methodInfo.Invoke(controller, new object[] { userId }) as string;
            var tokenHandler = new JwtSecurityTokenHandler();
            var parsedToken = tokenHandler.ReadJwtToken(result);
            var userIdClaim = parsedToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.NotNull(parsedToken);
            Assert.NotNull(userIdClaim);
            Assert.Equal(userId, userIdClaim.Value);
        }
    }
}
