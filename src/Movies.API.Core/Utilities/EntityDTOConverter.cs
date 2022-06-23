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
                    ActorName = entry.ActorName
                };
                actors.Add(actor);
            }

            return new MovieDTO()
            {
                Id = movie.Id,
                MovieName = movie.MovieName,
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
                    ActorName = entry.ActorName
                };
                actors.Add(actor);
            }
            return new Movie()
            {
                Id = movieDTO.Id,
                MovieName = movieDTO.MovieName,
                Rating = movieDTO.Rating,
                YearOfRelease = movieDTO.YearOfRelease,
                Actors = actors
            };
        }

        public static Actor ActorDTOToActor(ActorDTO actorDTO)
        {
            var movies = new List<Movie>();

            foreach (var entry in actorDTO.Movies)
            {
                var movie = new Movie()
                {
                    Id = entry.Id,
                    MovieName = entry.MovieName,
                    Rating = entry.Rating,
                    YearOfRelease = entry.YearOfRelease
                };
                movies.Add(movie);
            }
            return new Actor()
            {
                Id = actorDTO.Id,
                ActorName = actorDTO.ActorName
            };
        }
    }
}
