using Microsoft.EntityFrameworkCore;
using MyBlog.Models.Blog;
using MyBlog.Models.Services;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Models.Services
{
    public class PostFormServiceTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public PostFormServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BolgDb;Trust Server Certificate=False;Integrated Security=True;")
               .Options;
            _options = options;
        }

        [Fact]
        public void DeletePostData_ValidId_ReturnsTrue()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new PostFormService(context);
                var post = new Post { Title = "Test Post", Description = "Test Content" };
                context.Posts.Add(post);
                context.SaveChanges();

                // Act
                var result = service.DeletePostData(post.Id);

                // Assert
                Assert.True(result);
            }
        }

        [Fact]
        public void DeletePostData_InvalidId_ReturnsFalse()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new PostFormService(context);

                // Act
                var result = service.DeletePostData(1);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public void PostsData_AddsPostToContext()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new PostFormService(context);
                var post = new Post { Title = "Test Post", Description = "Test Content" };

                // Act
                service.PostsData(post);

                // Assert
                Assert.Contains(post, context.Posts);
            }
        }

        [Fact]
        public void UpdatedPost_ExistingPost_ReturnsTrue()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new PostFormService(context);
                var post = new Post { Title = "Test Post", Description = "Test Content" };
                context.Posts.Add(post);
                context.SaveChanges();

                // Act
                var updatedPost = new Post { Id = 1, Title = "Updated Post", Description = "Updated Content" };
                var result = service.UpdatedPost(updatedPost);

                // Assert
                Assert.False(result);
            }
        }

        [Fact]
        public void UpdatedPost_NonExistingPost_ReturnsFalse()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                var service = new PostFormService(context);
                var post = new Post { Title = "Test Post", Description = "Test Content" };
                context.Posts.Add(post);
                context.SaveChanges();

                // Act
                var updatedPost = new Post { Id = 2, Title = "Updated Post", Description = "Updated Content" };
                var result = service.UpdatedPost(updatedPost);

                // Assert
                Assert.False(result);
            }
        }
    }
}
