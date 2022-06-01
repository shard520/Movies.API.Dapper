using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Movies.API.Application.Interfaces;
using Movies.API.Controllers;
using Movies.API.Core.Entities;

namespace Movies.API.Tests
{
    public class MoviesControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly MoviesController _controller;

        public MoviesControllerTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _controller = new MoviesController(_unitOfWork.Object);
        }

        [Test]
        public async Task When_GetAll_Then_ReturnOkResponseWithListOfMovies()
        {
            // Arrange
            var movie1 = It.IsAny<Movie>();
            var movie2 = It.IsAny<Movie>();
            var expectedResult = new List<Movie>()
            {
                movie1, movie2
            };
            _unitOfWork.Setup(x => x.Movies.GetAllAsync())
                .ReturnsAsync(expectedResult);

            // Act
            var subject = await _controller.GetAll();
            var okResult = subject as OkObjectResult;

            // Assert
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task When_GetById_Then_ReturnOkResponseWithFoundMovie()
        {
            // Arrange
            var id = It.IsAny<int>();
            var expectedResult = Mock.Of<Movie>();
            _unitOfWork.Setup(x => x.Movies.GetByIdAsync(id))
                .ReturnsAsync(expectedResult);

            // Act
            var subject = await _controller.GetById(id);
            var okResult = subject as OkObjectResult;

            // Assert
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task When_GetByIdWithInvalidId_Then_ReturnNotFoundResponse()
        {
            // Arrange
            var invalidId = It.IsAny<int>();
            _unitOfWork.Setup(x => x.Movies.GetByIdAsync(invalidId))
                .ReturnsAsync(null as Movie);

            // Act
            var subject = await _controller.GetById(invalidId);
            var result = subject as NotFoundResult;

            // Assert
            subject.Should().BeOfType<NotFoundResult>();
            result.StatusCode.Should().Be(404);
        }

        [Test]
        public async Task When_Add_Then_ReturnOkResponseWithInt1()
        {
            // Arrange
            var movie = It.IsAny<Movie>();
            var expectedResult = 1;
            _unitOfWork.Setup(x => x.Movies.AddAsync(movie))
                .ReturnsAsync(expectedResult);

            // Act
            var subject = await _controller.Add(movie);
            var okResult = subject as OkObjectResult;

            // Assert
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task When_Delete_Then_ReturnOkResponseWithInt1()
        {
            // Arrange
            var id = It.IsAny<int>();
            var expectedResult = 1;
            _unitOfWork.Setup(x => x.Movies.DeleteAsync(id))
                .ReturnsAsync(expectedResult);

            // Act
            var subject = await _controller.Delete(id);
            var okResult = subject as OkObjectResult;

            // Assert
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task When_Update_Then_ReturnOkResponseWithInt1()
        {
            // Arrange
            var movie = It.IsAny<Movie>();
            var expectedResult = 1;
            _unitOfWork.Setup(x => x.Movies.UpdateAsync(movie))
                .ReturnsAsync(expectedResult);

            // Act
            var subject = await _controller.Update(movie);
            var okResult = subject as OkObjectResult;

            // Assert
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }
    }
}