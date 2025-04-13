using ManuelToleran_MVC_AgileProcess.Controllers;
using ManuelToleran_MVC_AgileProcess.Data;
using ManuelToleran_MVC_AgileProcess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    [TestClass]
    public class MoviesControllerTests
    {
        private ManuelToleran_MVC_AgileProcessContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ManuelToleran_MVC_AgileProcessContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            var context = new ManuelToleran_MVC_AgileProcessContext(options);

            if (!context.Movie.Any())
            {
                context.Movie.AddRange(
                    new Movie { Id = 1, Title = "Inception", Genre = "Sci-Fi", Price = 10.0m, Rating = "PG-13", ReleaseDate = new System.DateTime(2010, 7, 16) },
                    new Movie { Id = 2, Title = "Interstellar", Genre = "Sci-Fi", Price = 12.0m, Rating = "PG-13", ReleaseDate = new System.DateTime(2014, 11, 7) },
                    new Movie { Id = 3, Title = "The Dark Knight", Genre = "Action", Price = 15.0m, Rating = "PG-13", ReleaseDate = new System.DateTime(2008, 7, 18) }
                );
                context.SaveChanges();
            }

            return context;
        }

        [TestMethod]
        public async Task Index_ReturnsAViewResult_WithAListOfMovies()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);

            var result = await controller.Index(null, null, null, null);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as MovieGenreViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Movies.Count());
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);

            var result = await controller.Details(null);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsViewResult_WithMovie()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);

            var result = await controller.Details(1);

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as Movie;
            Assert.IsNotNull(model);
            Assert.AreEqual("Inception", model.Title);
        }

        [TestMethod]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);

            var movie = new Movie
            {
                Id = 4,
                Title = "Tenet",
                Genre = "Sci-Fi",
                Price = 11.0m,
                Rating = "PG-13",
                ReleaseDate = new System.DateTime(2020, 8, 26)
            };

            var result = await controller.Create(movie);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod]
        public async Task Edit_Post_ValidMovie_RedirectsToIndex()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);
            var movie = context.Movie.First();

            movie.Title = "Inception (Updated)";

            var result = await controller.Edit(movie.Id, movie);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);

            var updatedMovie = context.Movie.Find(movie.Id);
            Assert.AreEqual("Inception (Updated)", updatedMovie.Title);
        }

        [TestMethod]
        public async Task DeleteConfirmed_RemovesMovie_AndRedirects()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);
            int deleteId = 3;

            var result = await controller.DeleteConfirmed(deleteId);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.IsNull(context.Movie.Find(deleteId));
        }
    }
}