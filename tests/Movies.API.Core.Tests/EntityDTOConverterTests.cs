using Moq;
using FluentAssertions;
using Movies.API.Core.DTOs;
using Movies.API.Core.Entities;
using Movies.API.Core.Utilities;

namespace Movies.API.Core.Tests
{
    public class EntityDTOConverterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void When_GivenAMovie_Then_ReturnMovieDTO()
        {
            // Arrange
            var movie = Mock.Of<Movie>();

            // Act
            var result = EntityDTOConverter.MovieToMovieDTO(movie);

            // Assert
            result.Should().BeOfType<MovieDTO>();
        }

        [Test]
        public void When_GivenAMovieDTO_Then_ReturnMovie()
        {
            // Arrange
            var movieDTO = Mock.Of<MovieDTO>();

            // Act
            var result = EntityDTOConverter.MovieDTOToMovie(movieDTO);

            // Assert
            result.Should().BeOfType<Movie>();
        }

        [Test]
        public void When_GivenAnActorDTO_Then_ReturnActor()
        {
            // Arrange
            var actorDTO = Mock.Of<ActorDTO>();

            // Act
            var result = EntityDTOConverter.ActorDTOToActor(actorDTO);

            // Assert
            result.Should().BeOfType<Actor>();
        }
    }
}