using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;
using MovieRama;
using MovieRama.Controllers;
using System.Web;

namespace MovieRama.Tests.Controllers
{
    public class MoviesSetupControllerTest
    {
        [Fact]
        public void Index()
        {
            // Arrange
            var controller = new MoviesSetupController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Add()
        {
            // Arrange
            var controller = new MoviesSetupController();

            // Act
            ViewResult result = controller.Add() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        public static IEnumerable<object[]> AddFromFormData =>
        new List<object[]>
        {
            new object[] { new FormCollection() },
            new object[] { null },
        };
        [Theory]
        [MemberData(nameof(AddFromFormData))]
        public async Task AddFromForm(FormCollection col)
        {
            // Arrange
            var controller = new MoviesSetupController();

            // Act-Assert
            await Assert.ThrowsAsync<HttpException>(async () => await controller.Add(col));
        }

        [Fact]
        public void GetNullUserMovies()
        {
            // Arrange
            var controller = new MoviesSetupController();

            //Act
            var result = controller.MoviesByUser(null);

            // Assert
            Assert.NotNull(result);
        }
    }
}
