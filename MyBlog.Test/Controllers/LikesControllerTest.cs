using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using MyBlog.Models.Blog;
using MyBlog.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Controllers
{
    public class LikesControllerTest
    {
        [Fact]
        public async Task Likes_ValidLike_ReturnsOkResult()
        {
            // Arrange
            var likeData = new Mock<ILikesData>();

            likeData.Setup(ld => ld.postLike(It.IsAny<Like>())).Returns(true);

            var controller = new LikesController(likeData.Object);

            var like = new Like();

            // Act
            var result = await controller.Likes(like);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task Likes_InvalidLike_ReturnsNoContentResult()
        {
            // Arrange
            var likeData = new Mock<ILikesData>();
            likeData.Setup(ld => ld.postLike(It.IsAny<Like>())).Returns(false);

            var controller = new LikesController(likeData.Object);

            var like = new Like();

            // Act
            var result = await controller.Likes(like);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
