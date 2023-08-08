using Microsoft.EntityFrameworkCore;
using Moq;
using MyBlog.Models;
using MyBlog.Models.Blog;
using MyBlog.Models.Login;
using MyBlog.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Models.Services
{
    public class CommentsDataServiceTests
    {
        [Fact]
        public void GetComments_ReturnsListOfComments()
        {
            // Arrange
            var contextMock = new Mock<IApplicationDbContext>();
            var contextMock_ = new Mock<ApplicationDbContext>();
            var userMasters = new List<UserMaster>
            {
                new UserMaster { Id = 1, Name = "John Doe" },
                new UserMaster { Id = 2, Name = "Jane Smith" }
            };

            var userMastersDbSetMock = new Mock<DbSet<UserMaster>>();
            userMastersDbSetMock.As<IQueryable<UserMaster>>().Setup(m => m.Provider).Returns(userMasters.AsQueryable().Provider);
            userMastersDbSetMock.As<IQueryable<UserMaster>>().Setup(m => m.Expression).Returns(userMasters.AsQueryable().Expression);
            userMastersDbSetMock.As<IQueryable<UserMaster>>().Setup(m => m.ElementType).Returns(userMasters.AsQueryable().ElementType);
            userMastersDbSetMock.As<IQueryable<UserMaster>>().Setup(m => m.GetEnumerator()).Returns(userMasters.AsQueryable().GetEnumerator());
            contextMock.Setup(c => c.UserMasters).Returns(userMastersDbSetMock.Object);

            var comment = new List<Comment>
            {
                new Comment { PostId = 1, AuthorId = 1, Text = "This is a test comment1"},
                new Comment { PostId = 2, AuthorId = 1, Text = "This is a test comment2"},
            };

            var CommentDbSetMock = new Mock<DbSet<Comment>>();
            CommentDbSetMock.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(comment.AsQueryable().Provider);
            CommentDbSetMock.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(comment.AsQueryable().Expression);
            CommentDbSetMock.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(comment.AsQueryable().ElementType);
            CommentDbSetMock.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(comment.AsQueryable().GetEnumerator());
            contextMock.Setup(c => c.Comments).Returns(CommentDbSetMock.Object);

            var dataService = new CommentsDataService(contextMock_.Object,contextMock.Object);
            // Act
            var result = dataService.GetComments(1);

            // Assert
            Assert.NotNull(result);
        }
    }
}
