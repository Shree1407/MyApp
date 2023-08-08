using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Controllers
{
    public class WeatherForecastControllerTest
    {
        [Fact]
        public void Get_ReturnsListOfWeatherForecasts()
        {
            // Arrange
            var controller = new WeatherForecastController(Mock.Of<ILogger<WeatherForecastController>>());

            // Act
            var result = controller.Get();

            // Assert
            Assert.Equal(5, result.Count());
        }
        [Fact]
        public void Get_ReturnsListOfWeatherForecasts_WhenUserIsAuthorized()
        {
            // Arrange
            var logger = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.Name, "testuser"),
        new Claim(ClaimTypes.Role, "admin")
            }, "mock"));

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsType<WeatherForecast[]>(result);
            Assert.Equal(5, result.Count());
        }

        //[Fact]
        //public void Get_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        //{
        //    // Arrange
        //    var logger = new Mock<ILogger<WeatherForecastController>>();
        //    var controller = new WeatherForecastController(logger.Object);
        //    controller.ControllerContext = new ControllerContext();
        //    controller.ControllerContext.HttpContext = new DefaultHttpContext();

        //    // Act
        //    var result = controller.Get();

        //    // Assert
        //    Assert.IsType<UnauthorizedResult>(result);
        //}
    }
}
