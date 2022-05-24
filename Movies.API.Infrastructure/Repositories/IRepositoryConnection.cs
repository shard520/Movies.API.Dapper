using Movies.API.Core.Entities;
using System.Data;

namespace Movies.API.Infrastructure.Repositories
{
    public interface IRepositoryConnection
    {
        Task<int> ExecuteAsync(
            IDbConnection connection, string query, object queryObject);
        Task<T> QuerySingleOrDefaultAsync<T>(
            IDbConnection connection, string query, object queryObject);
        Task<IEnumerable<T>> QueryAsync<T>(
            IDbConnection connection, string query, object queryObject);
        Task<IEnumerable<T>> QueryAsync<T>(
            IDbConnection connection, string query);
        
    }
}