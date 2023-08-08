using Microsoft.EntityFrameworkCore;
using MyBlog.Models.Services;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Models.Blog;

namespace MyBlog.Test.Models.Services
{
    public class LikesDataServiceTests
    {
        private readonly LikesDataService _likesDataService;
        private readonly ApplicationDbContext _dbContext;

        public LikesDataServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BolgDb;Trust Server Certificate=False;Integrated Security=True;")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _likesDataService = new LikesDataService(_dbContext);
        }
        [Fact]
        public void PostLike_ShouldAddNewLikeAndIncrementNumLikes_WhenLikeDoesNotExist()
        {
            // Arrange
            var authorId = 10002;
            var postId = 10002;
            var like = new Like { AuthorId = authorId, PostID = postId };

            // Act
            var result = _likesDataService.postLike(like);

            // Assert
            Assert.True(result);

            var savedLike = _dbContext.Likes.FirstOrDefault(a => a.AuthorId == authorId && a.PostID == postId);
            Assert.NotNull(savedLike);

        }

        [Fact]
        public void PostLike_ShouldNotAddNewLikeAndNotIncrementNumLikes_WhenLikeAlreadyExists()
        {
            // Arrange
            var existingLike = new Like { AuthorId = 1, PostID = 1 };
            _dbContext.Likes.Add(existingLike);
            _dbContext.SaveChanges();

            var like = new Like { AuthorId = 1, PostID = 1 };

            // Act
            var result = _likesDataService.postLike(like);

            // Assert
            Assert.False(result);
        }

        
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
