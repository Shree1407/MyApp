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

namespace MyBlog.Test.Controllers
{
    public class BlogsControllerTest
    {
       
        [Fact]
        public void GetBlogs_ReturnsAllBlogs()
        {
            //AAA
            // Arrenge 
            var blogData = new Mock<IBlogData>();
            blogData.Setup(b => b.GetBlogs()).Returns(new List<BlogData>
            {
                new BlogData { Id = 1, Title = "Blog 1",Description="",AuthorId=1,DatePublished=DateTime.Now,numComments=0,numLikes=0,ImagePath=null,AuthorName="test" },
                new BlogData { Id = 2, Title = "Blog 2",Description="",AuthorId=1,DatePublished=DateTime.Now,numComments=0,numLikes=0,ImagePath=null,AuthorName="test" },
                new BlogData { Id = 3, Title = "Blog 3",Description="",AuthorId=1,DatePublished=DateTime.Now,numComments=0,numLikes=0,ImagePath=null,AuthorName="test" }
            });

            var controller = new BlogsController(blogData.Object);

            // Act
            var result = controller.Get();

            // Assert
            var blogs = Assert.IsAssignableFrom<IEnumerable<BlogData>>(result);
            Assert.Equal(3, blogs.Count());
                    

        }
        [Fact]
        public void GetById_ReturnsBlogWithMatchingId()
        {
            // Arrange
            var blogData = new Mock<IBlogData>();
            blogData.Setup(b => b.GetBlogsByid(1)).Returns(new List<BlogData>
            {
                new BlogData { Id = 1, Title = "Blog 1",Description="",AuthorId=1,DatePublished=DateTime.Now,numComments=0,numLikes=0,ImagePath=null,AuthorName="test" },
            });

            var controller = new BlogsController(blogData.Object);

            // Act
            var result = controller.ByIdBlogs(1);

            // Assert
            var blogs = Assert.IsAssignableFrom<IEnumerable<BlogData>>(result);
            Assert.Single(blogs);
            Assert.Equal(1, blogs.First().Id);
        }

    }
}
