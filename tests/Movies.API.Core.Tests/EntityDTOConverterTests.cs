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
            var actor = Mock.Of<Actor>();
            movie.Actors.Add(actor);

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
            var actorDTO = Mock.Of<ActorDTO>();
            movieDTO.Actors.Add(actorDTO);

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
            var movieDTO = Mock.Of<MovieDTO>();
            actorDTO.Movies.Add(movieDTO);

            // Act
            var result = EntityDTOConverter.ActorDTOToActor(actorDTO);

            // Assert
            result.Should().BeOfType<Actor>();
        }
    }
}