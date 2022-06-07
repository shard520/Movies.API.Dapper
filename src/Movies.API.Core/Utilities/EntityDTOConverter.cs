using Movies.API.Core.DTOs;
using Movies.API.Core.Entities;

namespace Movies.API.Core.Utilities
{
    public static class EntityDTOConverter
    {
        public static MovieDTO MovieToMovieDTO(Movie movie)
        {
            var actors = new List<ActorDTO>();

            foreach(var entry in movie.Actors)
            {
                var actor = new ActorDTO()
                {
                    Id = entry.Id,
                    Name = entry.Name
                };
                actors.Add(actor);
            }

            return new MovieDTO()
            {
                Id = movie.Id,
                Name = movie.Name,
                Rating = movie.Rating,
                YearOfRelease = movie.YearOfRelease,
                Actors = actors
            };
        }

        public static Movie MovieDTOToMovie(MovieDTO movieDTO)
        {
            var actors = new List<Actor>();

            foreach (var entry in movieDTO.Actors)
            {
                var actor = new Actor()
                {
                    Id = entry.Id,
                    Name = entry.Name
                };
                actors.Add(actor);
            }
            return new Movie()
            {
                Id = movieDTO.Id,
                Name = movieDTO.Name,
                Rating = movieDTO.Rating,
                YearOfRelease = movieDTO.YearOfRelease,
                Actors = actors
            };
        }
    }
}
