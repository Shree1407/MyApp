using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBlog.Models.Blog;
using MyBlog.Models.Login;
using MyBlog.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Controllers
{
    public class PostFormControllerTests
    {
        [Fact]
        public async Task PostForm_ValidData_ReturnsOkResult()
        {
            // Arrange
            var formDataMock = new Mock<IPostFormData>();
            var controller = new PostFormController(formDataMock.Object);
            var formFileMock = new Mock<IFormFile>();
            var createPost = new CreatePost
            {
                Title = "Test Post",
                Description = "This is a test post",
                AuthorId = 1,
                Image = formFileMock.Object
            };
            formDataMock.Setup(x => x.PostsData(It.IsAny<Post>())).Verifiable();

            // Act
            var result = await controller.PostForm(createPost) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            formDataMock.Verify(x => x.PostsData(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public void DeletePostForm_ExistingPost_ReturnsOkResult()
        {
            // Arrange
            var formDataMock = new Mock<IPostFormData>();
            var controller = new PostFormController(formDataMock.Object);
            var postId = 1;
            formDataMock.Setup(x => x.DeletePostData(postId)).Returns(true);

            // Act
            var result = controller.DeletePostForm(postId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void DeletePostForm_NonExistingPost_ReturnsNoContentResult()
        {
            // Arrange
            var formData = new Mock<IPostFormData>();
            formData.Setup(f => f.DeletePostData(It.IsAny<int>())).Returns(false);
            var controller = new PostFormController(formData.Object);

            // Act
            var result = controller.DeletePostForm(1);

            // Assert
            Assert.IsType<NoContentResult>(result);

            formData.Verify(f => f.DeletePostData(1), Times.Once);
        }

        [Fact]
        public void UpdatedPostForm_ValidData_ReturnsOkResult()
        {
            // Arrange
            var formData = new Mock<IPostFormData>();
            formData.Setup(f => f.UpdatedPost(It.IsAny<Post>())).Returns(true);
            var controller = new PostFormController(formData.Object);

            var createPost = new Post
            {
                Id = 1,
                Title = "Updated Post"
            };

            // Act
            var result = controller.UpdatedPostForm(createPost);

            // Assert
            Assert.IsType<OkObjectResult>(result);

            formData.Verify(f => f.UpdatedPost(createPost), Times.Once);
        }

        [Fact]
        public void UpdatedPostForm_InvalidData_ReturnsBadRequestResult()
        {
            // Arrange
            var formData = new Mock<IPostFormData>();
            formData.Setup(f => f.UpdatedPost(It.IsAny<Post>())).Returns(false);
            var controller = new PostFormController(formData.Object);

            var createPost = new Post
            {
                Id = 1,
                Title = "Updated Post"
            };

            // Act
            var result = controller.UpdatedPostForm(createPost);

            // Assert
            Assert.IsType<BadRequestResult>(result);

            formData.Verify(f => f.UpdatedPost(createPost), Times.Once);
        }
        [Fact]
        public void UploadImage_ValidImage_ReturnsImagePath()
        {
            // Arrange
            var formDataMock = new Mock<IPostFormData>();
            var controller = new PostFormController(formDataMock.Object);
            var formFileMock = new Mock<IFormFile>();
            var fileName = "test.jpg";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

            formFileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), CancellationToken.None)).Callback<Stream, CancellationToken>((stream, token) =>
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    // Write test data to the file stream
                    byte[] testData = Encoding.UTF8.GetBytes("Test image data");
                    fileStream.Write(testData, 0, testData.Length);
                }
            }).Returns(Task.CompletedTask);

            var uploadImageMethod = typeof(PostFormController).GetMethod("UploadImage", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = uploadImageMethod.Invoke(controller, new object[] { formFileMock.Object }) as Task<string>;

            // Assert
            Assert.NotNull(result);
        }
    }
}
