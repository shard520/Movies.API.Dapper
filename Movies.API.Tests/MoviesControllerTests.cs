using Moq;
using Movies.API.Application.Interfaces;
using Movies.API.Controllers;
using Movies.API.Core.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Movies.API.Infrastructure;

namespace Movies.API.Tests
{
    public class MoviesControllerTests
    {
        private readonly Mock<IMovieRepository> _movieRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly MoviesController _controller;

        public MoviesControllerTests()
        {
            _movieRepository = new Mock<IMovieRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _controller = new MoviesController(_unitOfWork.Object);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task When_GetAll_Then_ReturnOkResponseWithListOfMovies()
        {
            // Arrange
            var movie1 = Mock.Of<Movie>();
            var movie2 = Mock.Of<Movie>();
            var expectedResult = new List<Movie>()
            {
                movie1, movie2
            };
            _unitOfWork.Setup(x => x.Movies.GetAllAsync())
                .ReturnsAsync(expectedResult).Verifiable();

            // Act
            var subject = await _controller.GetAll();
            var okResult = subject as OkObjectResult;
            subject.Should().BeOfType<OkObjectResult>();
            // Assert
            okResult.Should().NotBeNull();
            var result = okResult.Value;
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}