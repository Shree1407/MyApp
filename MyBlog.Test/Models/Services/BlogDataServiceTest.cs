using Moq;
using MyBlog.Models.Blog;
using MyBlog.Models.Login;
using MyBlog.Models.Services;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.Test.Models.Services
{

    public class BlogDataServiceTests
    {
        [Fact]
        public void GetBlogs_ReturnsListOfBlogData()
        {
            // Arrange
            var contextMock = new Mock<IApplicationDbContext>();
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

            var posts = new List<Post>
            {
                new Post { Id = 1, AuthorId = 1, Title = "Blog Post 1" },
                new Post { Id = 2, AuthorId = 2, Title = "Blog Post 2" },
                new Post { Id = 3, AuthorId = 1, Title = "Blog Post 3" }
            };
            var postsDbSetMock = new Mock<DbSet<Post>>();
            postsDbSetMock.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.AsQueryable().Provider);
            postsDbSetMock.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.AsQueryable().Expression);
            postsDbSetMock.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.AsQueryable().ElementType);
            postsDbSetMock.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.AsQueryable().GetEnumerator());
            contextMock.Setup(c => c.Posts).Returns(postsDbSetMock.Object);

            var dataService = new BlogDataService(contextMock.Object);

            // Act
            var result = dataService.GetBlogs();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<BlogData>>(result);
        }
        [Fact]
        public void GetBlogsByid_ReturnsListOfBlogData()
        {
            // Arrange
            var contextMock = new Mock<IApplicationDbContext>();
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

            var posts = new List<Post>
            {
                new Post { Id = 1, AuthorId = 1, Title = "Blog Post 1" },
                new Post { Id = 2, AuthorId = 2, Title = "Blog Post 2" },
                new Post { Id = 3, AuthorId = 1, Title = "Blog Post 3" }
            };
            var postsDbSetMock = new Mock<DbSet<Post>>();
            postsDbSetMock.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(posts.AsQueryable().Provider);
            postsDbSetMock.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(posts.AsQueryable().Expression);
            postsDbSetMock.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(posts.AsQueryable().ElementType);
            postsDbSetMock.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(posts.AsQueryable().GetEnumerator());
            contextMock.Setup(c => c.Posts).Returns(postsDbSetMock.Object);

            var dataService = new BlogDataService(contextMock.Object);

            // Act
            var result = dataService.GetBlogsByid(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<BlogData>>(result);
        }
    }
}
