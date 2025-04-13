using ManuelToleran_MVC_AgileProcess.Data;
using ManuelToleran_MVC_AgileProcess.Models;
using Microsoft.EntityFrameworkCore;

namespace Foysal_UnitTest
{
    public class MoviesControllerTests
    {
        private ManuelToleran_MVC_AgileProcessContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ManuelToleran_MVC_AgileProcessContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new ManuelToleran_MVC_AgileProcessContext(options);

            // Seed sample data
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

        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfMovies()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);

            var result = await controller.Index(null, null, null, null);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MovieGenreViewModel>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Movies.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithMovie()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);

            var result = await controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Movie>(viewResult.ViewData.Model);
            Assert.Equal("Inception", model.Title);
        }

        [Fact]
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

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ValidMovie_RedirectsToIndex()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);
            var movie = context.Movie.First();

            movie.Title = "Inception (Updated)";

            var result = await controller.Edit(movie.Id, movie);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            var updatedMovie = context.Movie.Find(movie.Id);
            Assert.Equal("Inception (Updated)", updatedMovie.Title);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesMovie_AndRedirects()
        {
            var context = GetDbContext();
            var controller = new MoviesController(context);
            int deleteId = 3;

            var result = await controller.DeleteConfirmed(deleteId);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Null(context.Movie.Find(deleteId));
        }
    }
}