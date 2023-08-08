using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBlog.Models.Blog;
using MyBlog.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MyBlog.Test.Controllers
{
    public class CommentControllerTest
    {
        [Fact]
        public void GetComments_ReturnsAllBlogs()
        {
            //AAA
            // Arrenge 
            var commentData = new Mock<ICommentsData>();
            commentData.Setup(b => b.GetComments(1)).Returns(new List<GetComments>
            {
                new GetComments { Id = 1,Text = "comment 1",PostId = 1, AuthorId=1,DatePublished=DateTime.Now,AuthorName="test" },
                new GetComments { Id = 2,Text = "comment 2",PostId = 1, AuthorId=1,DatePublished=DateTime.Now,AuthorName="test" },
                new GetComments { Id = 3,Text = "comment 3",PostId = 1, AuthorId=1,DatePublished=DateTime.Now,AuthorName="test" }
            });

            var controller = new CommentController(commentData.Object);

            // Act
            var result = controller.Get(1);

            // Assert
            var comments = Assert.IsAssignableFrom<ActionResult<IEnumerable<GetComments>>>(result);
            Assert.IsType<ActionResult<IEnumerable<GetComments>>>(comments);


        }
        [Fact]
        public void PostComments_ReturnsLastRecord()
        {
            // Arrange
            var commentData = new Mock<ICommentsData>();
            var _comments = new Comment
            {
                Id = 1,
                Text = "comment 3",
                PostId = 1,
                AuthorId = 1,
                DatePublished = DateTime.Now,
            };
            commentData.Setup(b => b.PostComments(_comments)).Returns(
                new GetComments { Id = 1, Text = "comment 3", PostId = 1, AuthorId = 1, DatePublished = DateTime.Now, AuthorName = "test", });

            var controller = new CommentController(commentData.Object);

            // Act
            var result = controller.post(_comments);

            // Assert
            var comment = Assert.IsAssignableFrom<ActionResult<GetComments>>(result);
            Assert.IsType<ActionResult<GetComments>>(comment);
        }
    }
}
