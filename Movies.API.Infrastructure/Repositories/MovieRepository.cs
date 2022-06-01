using Dapper;
using Microsoft.Extensions.Configuration;
using Movies.API.Application.Interfaces;
using Movies.API.Core.Entities;
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

        public async Task<int> AddAsync(Movie movie)
        {
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

        public async Task<IReadOnlyList<Movie>> GetAllAsync()
        {
            var sql = @"SELECT [m].[Id], 
	                        [m].[CreatedDate], 
	                        [m].[Name], 
	                        [m].[Rating], 
	                        [m].[UpdatedDate], 
	                        [m].[YearOfRelease], 
	                        [t].[ActorId], 
	                        [t].[MovieId], 
	                        [t].[Id], 
	                        [t].[CreatedDate], 
	                        [t].[Name], 
	                        [t].[UpdatedDate]
                        FROM [MoviesDb].[dbo].Movies AS [m]
                        LEFT JOIN (
	                        SELECT [m0].[ActorId], [m0].[MovieId], [a].[Id], [a].[CreatedDate], [a].[Name], [a].[UpdatedDate]
                            FROM [MoviesDb].[dbo].[MovieActors] AS [m0]
                            INNER JOIN [MoviesDb].[dbo].[Actors] AS [a] ON [m0].[ActorId] = [a].[Id]
                            ) 
                        AS [t] ON [m].[Id] = [t].[MovieId]
                        ORDER BY [m].[Id], [t].[ActorId], [t].[MovieId]";
            using (var connection = GetConnection())
            {
                connection.Open();
                var movies = await connection.QueryAsync<Movie, Actor, Movie>(
                    sql,
                    (movie, actor) => {
                        movie.Actors.Add(actor);
                        return movie;
                    }, 
                    splitOn: "ActorId");
                var result = movies.GroupBy(m => m.Id).Select(x =>
                {
                    var groupedMovie = x.First();
                    groupedMovie.Actors = x.Select(y => y.Actors.Single()).ToList();
                    return groupedMovie;
                });
                return result.ToList();
            }
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Movies WHERE Id = @Id";
            using (var connection = GetConnection())
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<Movie>(
                    sql, new { Id = id});
            }
        }

        public async Task<int> UpdateAsync(Movie movie)
        {
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
