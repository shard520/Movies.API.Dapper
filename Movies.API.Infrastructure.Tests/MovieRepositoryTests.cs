using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Movies.API.Core.Entities;
using Movies.API.Infrastructure.Repositories;
using System.Data;

namespace Movies.API.Infrastructure.Tests
{
    public class MovieRepositoryTests
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IDbConnection> _dbConnection;
        private readonly Mock<IRepositoryConnection> _repositoryConnection;
        private readonly MovieRepository _movieRepository;
        public MovieRepositoryTests()
        {
            _configuration = new Mock<IConfiguration>();
            _dbConnection = new Mock<IDbConnection>();
            _repositoryConnection = new Mock<IRepositoryConnection>();
            _movieRepository = new MovieRepository(
                _configuration.Object,
                _repositoryConnection.Object);
        }

        [Test]
        public async Task When_AddAsync_Then_ReturnIntFromCompletedTask()
        {
            // Arrange
            var expectedResult = 1;
            var movie = Mock.Of<Movie>();

            _repositoryConnection.Setup(r => r.ExecuteAsync(
                _dbConnection.Object,
                It.IsAny<string>(),
                movie))
                .Returns(Task.FromResult(expectedResult));

            //_movieRepository
            //    .Protected()
            //    .Setup<IDbConnection>("GetConnection")
            //    .Returns(_dbConnection.Object);

            // Act
            var result = await _movieRepository.AddAsync(movie);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public async Task When_DeleteAsync_Then_ReturnIntFromCompletedTask()
        {
            // Arrange
            var expectedResult = 1;
            
            _repositoryConnection.Setup(r => r.ExecuteAsync(
                _dbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
                .Returns(Task.FromResult(expectedResult));

            //_movieRepository
            //    .Protected()
            //    .Setup<IDbConnection>("GetConnection")
            //    .Returns(_dbConnection.Object);

            // Act
            var result = await _movieRepository.DeleteAsync(It.IsAny<int>());

            // Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public async Task When_GetAllAsync_Then_ReturnListOfMovies()
        {
            // Arrange
            var movie1 = Mock.Of<Movie>();
            var movie2 = Mock.Of<Movie>();
            var expectedResult = new List<Movie>()
            {
                movie1, movie2
            };
            
            _repositoryConnection.Setup(r => r.QueryAsync<Movie>(
                _dbConnection.Object, It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<Movie>>(expectedResult));

            //_movieRepository
            //    .Protected()
            //    .Setup<IDbConnection>("GetConnection")
            //    .Returns(_dbConnection.Object);

            // Act
            var result = await _movieRepository.GetAllAsync();

            // Assert
            result.Should().ContainItemsAssignableTo<Movie>();
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task When_GetByIdAsync_Then_Return_Movie()
        {
            // Arrange
            var expectedResult = It.IsAny<Movie>();

            _repositoryConnection.Setup(r => r.QuerySingleOrDefaultAsync<Movie>(
                _dbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<int>()))
                .Returns(Task.FromResult(expectedResult));

            //_movieRepository
            //    .Protected()
            //    .Setup<IDbConnection>("GetConnection")
            //    .Returns(_dbConnection.Object);

            // Act
            var result = await _movieRepository.GetByIdAsync(It.IsAny<int>());

            // Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public async Task When_UpdateAsync_Then_ReturnIntFromCompletedTask()
        {
            // Arrange
            var expectedResult = 1;

            _repositoryConnection.Setup(r => r.ExecuteAsync(
                _dbConnection.Object,
                It.IsAny<string>(),
                It.IsAny<object>()))
                .Returns(Task.FromResult(expectedResult));

            //_movieRepository
            //    .Protected()
            //    .Setup<IDbConnection>("GetConnection")
            //    .Returns(_dbConnection.Object);

            // Act
            var result = await _movieRepository.UpdateAsync(Mock.Of<Movie>());

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}