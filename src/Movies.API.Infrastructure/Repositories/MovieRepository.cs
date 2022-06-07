using Dapper;
using Microsoft.Extensions.Configuration;
using Movies.API.Application.Interfaces;
using Movies.API.Core.DTOs;
using Movies.API.Core.Entities;
using Movies.API.Core.Utilities;
using Serilog;
using System.Data;
using System.Data.SqlClient;

namespace Movies.API.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IConfiguration _configuration;

        public MovieRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection GetConnection()
        {
            return new SqlConnection(_configuration
                .GetConnectionString("DefaultConnection"));
        }

        public async Task<int> AddAsync(MovieDTO movieDTO)
        {
            var movie = EntityDTOConverter.MovieDTOToMovie(movieDTO);
            movie.CreatedDate = DateTimeOffset.Now;
            var sql = "INSERT INTO Movies(Name,YearOfRelease,Rating,CreatedDate) VALUES (@Name,@YearOfRelease,@Rating,@CreatedDate)";
            using (var connection = GetConnection())
            {
                connection.Open();
                return await connection.ExecuteAsync(sql, movie);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Movies WHERE Id = @Id";
            using (var connection = GetConnection())
            {
                connection.Open();
                return await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<IReadOnlyList<MovieDTO>> GetAllAsync()
        {
            var procedure = "[dbo].[GetMoviesWithActors]";
            using (var connection = GetConnection())
            {
                connection.Open();
                var movies = await connection.QueryAsync<Movie, Actor, Movie>(
                    procedure,
                    (movie, actor) => {
                        movie.Actors.Add(actor);
                        return movie;
                    }, 
                    splitOn: "ActorId",
                    commandType: CommandType.StoredProcedure);
                var groupedMovies = movies.GroupBy(m => m.Id).Select(x =>
                {
                    var groupedMovie = x.First();
                    groupedMovie.Actors = x.Select(y => y.Actors.Single()).ToList();
                    return groupedMovie;
                });

                var result = new List<MovieDTO>();
                foreach(var movie in groupedMovies)
                {
                    result.Add(EntityDTOConverter.MovieToMovieDTO(movie));
                }

                return result;
            }
        }

        public async Task<MovieDTO> GetByIdAsync(int id)
        {
            var procedure = "[dbo].[GetMovieByIdWithActors]";

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var movies = await connection.QueryAsync<Movie, Actor, Movie>(
                        procedure,
                        (movie, actor) =>
                        {
                            movie.Actors = new List<Actor>()
                            {
                            actor
                            };
                            return movie;
                        },
                        new { Id = id },
                        splitOn: "ActorId", 
                        commandType: CommandType.StoredProcedure);
                    var result = movies.GroupBy(m => m.Id).Select(x =>
                    {
                        var groupedMovie = x.First();
                        groupedMovie.Actors = x.Select(y => y.Actors.Single()).ToList();
                        return groupedMovie;
                    }).ToList<Movie>();

                    return EntityDTOConverter.MovieToMovieDTO(result[0]);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                if(ex.Message.Contains("Index was out of range."))
                {
                    return null;
                }
                throw;
            }

            
        }

        public async Task<int> UpdateAsync(MovieDTO movieDTO)
        {
            var movie = EntityDTOConverter.MovieDTOToMovie(movieDTO);
            movie.UpdatedDate = DateTimeOffset.Now;
            var sql = "UPDATE Movies SET Name = @Name, YearOfRelease = @YearOfRelease, Rating = @Rating, UpdatedDate = @UpdatedDate WHERE Id = @Id";
            using (var connection = GetConnection())
            {
                connection.Open();
                return await connection.ExecuteAsync(sql, movie);
            }
        }
    }
}
