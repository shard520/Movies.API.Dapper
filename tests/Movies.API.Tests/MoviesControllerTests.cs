using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Movies.API.Application.Interfaces;
using Movies.API.Controllers;
using Movies.API.Core.DTOs;

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
        public async Task When_AddNewMovie_Then_ReturnOkResponseWithId()
        {
            // Arrange
            var movieDTO = It.IsAny<MovieDTO>();
            var expectedResult = 1;
            _unitOfWork.Setup(x => x.Movies.AddAsync(movieDTO))
                .ReturnsAsync(expectedResult);

            // Act
            var subject = await _controller.Add(movieDTO);
            var okResult = subject as OkObjectResult;

            // Assert
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task When_AddExistingMovie_Then_ReturnConflictWithNameInMessage()
        {
            // Arrange
            var movieDTO = Mock.Of<MovieDTO>();
            movieDTO.MovieName = It.IsAny<string>();
            var existingId = It.IsAny<int>();
            var expectedMessage = $"A movie with the name \"{movieDTO.MovieName}\" already exists with an id of {existingId}. Please update the existing movie.";
            _unitOfWork.Setup(x => x.Movies.AddAsync(movieDTO))
                .ThrowsAsync(new Exception ($"{existingId}"));

            // Act
            var subject = await _controller.Add(movieDTO);
            var conflictResult = subject as ConflictObjectResult;

            // Assert
            conflictResult.StatusCode.Should().Be(409);
            conflictResult.Value.Should().Be(expectedMessage);
        }

        [Test]
        public async Task When_GetAll_Then_ReturnOkResponseWithListOfMovies()
        {
            // Arrange
            var movie1 = It.IsAny<MovieDTO>();
            var movie2 = It.IsAny<MovieDTO>();
            var expectedResult = new List<MovieDTO>()
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
            var expectedResult = Mock.Of<MovieDTO>();
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
        public async Task When_GetByIdWithInvalidId_Then_ReturnNotFoundResponseWithIdInMessage()
        {
            // Arrange
            var invalidId = It.IsAny<int>();
            var expectedMessage = $"Movie with id {invalidId} not found.";
            _unitOfWork.Setup(x => x.Movies.GetByIdAsync(invalidId))
                .ReturnsAsync(null as MovieDTO);

            // Act
            var subject = await _controller.GetById(invalidId);
            var result = subject as NotFoundObjectResult;
            
            // Assert
            result.StatusCode.Should().Be(404);
            result.Value.Should().Be(expectedMessage);
        }

        [Test]
        public async Task When_Update_Then_ReturnOkResponseWithUpdatedMovie()
        {
            // Arrange
            var movieDTO = Mock.Of<MovieDTO>();
            var expectedResult = movieDTO;
            _unitOfWork.Setup(x => x.Movies.UpdateAsync(movieDTO))
                .ReturnsAsync(expectedResult);

            // Act
            var subject = await _controller.Update(movieDTO);
            var okResult = subject as OkObjectResult;

            // Assert
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task When_UpdateNonExistingMovie_Then_ReturnNotFoundResponseWithMessage()
        {
            // Arrange
            var movieDTO = Mock.Of<MovieDTO>();
            var expectedMessage = "Update operation was unsuccesful, please check the id supplied is valid.";
            _unitOfWork.Setup(x => x.Movies.UpdateAsync(movieDTO))
                .ReturnsAsync(null as MovieDTO);

            // Act
            var subject = await _controller.Update(movieDTO);
            var notFoundResult = subject as NotFoundObjectResult;

            // Assert
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be(expectedMessage);
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
        public async Task When_DeleteNonExistingMovie_Then_ReturnNotFoundResponseWithMessage()
        {
            // Arrange
            var id = It.IsAny<int>();
            var failedDeleteCode = 0;
            var expectedMessage = "Delete operation was unsuccesful, please check the id supplied is valid.";
            _unitOfWork.Setup(x => x.Movies.DeleteAsync(id))
                .ReturnsAsync(failedDeleteCode);

            // Act
            var subject = await _controller.Delete(id);
            var notFoundResult = subject as NotFoundObjectResult;

            // Assert
            notFoundResult.StatusCode.Should().Be(404);
            notFoundResult.Value.Should().Be(expectedMessage);
        }
    }
}