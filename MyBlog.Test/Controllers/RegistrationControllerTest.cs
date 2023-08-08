using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBlog.Models.Login;
using MyBlog.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Controllers
{
    public class RegistrationControllerTest
    {
        [Fact]
        public void Registration_ReturnsOkResult_WhenUserIsSuccessfullyRegistered()
        {
            // Arrange
            var registrationDataMock = new Mock<IRegistrationData>();
            registrationDataMock.Setup(x => x.GetUserMasterByEmail(It.IsAny<string>())).Returns((UserMaster)null);

            var controller = new RegistrationController(registrationDataMock.Object);
            var registrationModel = new UserMaster
            {
                Name = "John Doe",
                Age = 25,
                Gender = "Male",
                Mobile = "1234567890",
                Email = "john.doe@example.com",
                Password = "password"
            };

            // Act
            var result = controller.Registration(registrationModel);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            registrationDataMock.Verify(x => x.PostUserMaster(It.IsAny<UserMaster>()), Times.Once);
        }
        [Fact]
        public void Registration_ReturnsBadRequest_WhenUserEmailIsAlreadyTaken()
        {
            // Arrange
            var registrationDataMock = new Mock<IRegistrationData>();
            registrationDataMock.Setup(x => x.GetUserMasterByEmail(It.IsAny<string>())).Returns(new UserMaster());

            var controller = new RegistrationController(registrationDataMock.Object);
            var registrationModel = new UserMaster
            {
                Name = "John Doe",
                Age = 25,
                Gender = "Male",
                Mobile = "1234567890",
                Email = "john.doe@example.com",
                Password = "password"
            };

            // Act
            var result = controller.Registration(registrationModel);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            registrationDataMock.Verify(x => x.PostUserMaster(It.IsAny<UserMaster>()), Times.Never);
        }
    }
}
