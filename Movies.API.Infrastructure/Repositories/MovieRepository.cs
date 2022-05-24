using Dapper;
using Microsoft.Extensions.Configuration;
using Movies.API.Application.Interfaces;
using Movies.API.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.API.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IRepositoryConnection _repositoryConnection;
        private readonly IConfiguration _configuration;

        public MovieRepository(IConfiguration configuration, IRepositoryConnection repositoryConnection)
        {
            _configuration = configuration;
            _repositoryConnection = repositoryConnection;
        }
        protected virtual IDbConnection GetConnection()
        {
            return new SqlConnection(_configuration
                .GetConnectionString("DefaultConnection"));
        }

        public async Task<int> AddAsync(Movie movie)
        {
            movie.CreatedDate = DateTimeOffset.Now;
            var sql = "INSERT INTO Movies(Name,YearOfRelease,Rating,CreatedDate) VALUES (@Name,@YearOfRelease,@Rating,@CreatedDate)";
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                return await _repositoryConnection.ExecuteAsync(
                    dbConnection, sql, movie);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Movies WHERE Id = @Id";
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                return await _repositoryConnection.ExecuteAsync(
                    dbConnection, sql, new { Id = id });
            }
        }

        public async Task<IReadOnlyList<Movie>> GetAllAsync()
        {
            var sql = "SELECT * FROM Movies";
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                var result = await _repositoryConnection.QueryAsync<Movie>(
                    dbConnection, sql);
                return result.ToList();
            }
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Movies WHERE Id = @Id";
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                return await _repositoryConnection
                    .QuerySingleOrDefaultAsync<Movie>(
                    dbConnection, sql, id);
            }
        }

        public async Task<int> UpdateAsync(Movie entity)
        {
            entity.UpdatedDate = DateTimeOffset.Now;
            var sql = "UPDATE Movies SET Name = @Name, YearOfRelease = @YearOfRelease, Rating = @Rating, UpdatedDate = @UpdatedDate WHERE Id = @Id";
            using (var dbConnection = GetConnection())
            {
                dbConnection.Open();
                return await _repositoryConnection.ExecuteAsync(
                    dbConnection, sql, entity);
            }
        }
    }
}
