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
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    await CheckForExistingMovie(movieDTO, connection);

                    using (var transaction = connection.BeginTransaction())
                    {
                        var movieId = await InsertAndGetId(
                            new
                            {
                                movieDTO.MovieName,
                                movieDTO.YearOfRelease,
                                movieDTO.Rating
                            },
                            "[dbo].[InsertMovie]",
                            connection,
                            transaction
                            );

                        foreach (var actor in movieDTO.Actors)
                        {
                            var actorId = await InsertAndGetId(
                                new { actor.ActorName },
                                "[dbo].[InsertActor]",
                                connection,
                                transaction
                                );

                            await connection.ExecuteAsync(
                                "[dbo].[InsertMovieActor]",
                                new { MovieId = movieId, ActorId = actorId },
                                transaction,
                                commandType: CommandType.StoredProcedure
                                );
                        }

                        transaction.Commit();
                        return movieId;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
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
                    (movie, actor) =>
                    {
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
                foreach (var movie in groupedMovies)
                {
                    result.Add(EntityDTOConverter.MovieToMovieDTO(movie));
                }

                return result;
            }
        }

        public async Task<MovieDTO?> GetByIdAsync(int id)
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
                    }).ToList<Movie>().First<Movie>();

                    return EntityDTOConverter.MovieToMovieDTO(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                if (ex.Message.Contains("Sequence contains no elements"))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<MovieDTO?> UpdateAsync(MovieDTO movieDTO)
        {
            var movie = EntityDTOConverter.MovieDTOToMovie(movieDTO);

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        foreach (var actor in movieDTO.Actors)
                        {
                            var actorId = await InsertAndGetId(
                                new { actor.ActorName },
                                "[dbo].[InsertActor]",
                                connection,
                                transaction
                                );

                            await connection.ExecuteAsync(
                                "[dbo].[InsertMovieActor]",
                                new { MovieId = movieDTO.Id, ActorId = actorId },
                                transaction,
                                commandType: CommandType.StoredProcedure
                                );
                        }

                        transaction.Commit();
                    }


                    var movies = await connection.QueryAsync<Movie, Actor, Movie>(
                        "[dbo].[UpdateMovie]",
                        (movie, actor) =>
                        {
                            movie.Actors = new List<Actor>()
                            {
                            actor
                            };
                            return movie;
                        },
                        new { movieDTO.Id, movieDTO.MovieName, movieDTO.YearOfRelease, movieDTO.Rating },
                        splitOn: "ActorId",
                        commandType: CommandType.StoredProcedure);
                    var result = movies.GroupBy(m => m.Id).Select(x =>
                    {
                        var groupedMovie = x.First();
                        groupedMovie.Actors = x.Select(y => y.Actors.Single()).ToList();
                        return groupedMovie;
                    }).First<Movie>();

                    return EntityDTOConverter.MovieToMovieDTO(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                if (ex.Message.Contains("The MERGE statement conflicted with the FOREIGN KEY constraint"))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    var queryParam = new DynamicParameters(new { Id = id });
                    queryParam.Add("@rval", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                    await connection.ExecuteAsync("DeleteMovieAndMovieActors", queryParam, commandType: CommandType.StoredProcedure);
                    return queryParam.Get<int>("@rval");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw;
            }
        }

        private static async Task<int> InsertAndGetId(object rowValues, string procedureName, IDbConnection connection, IDbTransaction transaction)
        {
            var queryParam = new DynamicParameters(rowValues);
            queryParam.Add("@rval", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteScalarAsync<int>(
                procedureName,
                queryParam,
                transaction,
                commandType: CommandType.StoredProcedure
                );

            return queryParam.Get<int>("@rval");
        }

        private static async Task CheckForExistingMovie(MovieDTO movieDTO, IDbConnection connection)
        {
            var queryParam = new DynamicParameters(new { movieDTO.MovieName });
            queryParam.Add("@rval", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await connection.ExecuteAsync("GetMovieIdFromName", queryParam, commandType: CommandType.StoredProcedure);
            int existingId = queryParam.Get<int>("@rval");
            if (existingId != 0)
            {
                Log.Error("Movie with name \"{name}\" already exists, transaction aborted", movieDTO.MovieName);
                throw new Exception($"{existingId}");
            }
        }
    }
}
