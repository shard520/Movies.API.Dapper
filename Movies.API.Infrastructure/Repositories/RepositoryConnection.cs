using Dapper;
using Microsoft.Extensions.Configuration;
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
    public class RepositoryConnection : IRepositoryConnection
    {
        public async Task<int> ExecuteAsync(
            IDbConnection connection, string query, object queryObject)
        {
            return await connection.ExecuteAsync(query, queryObject);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(
            IDbConnection connection, string query, object queryObject)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(
                query, queryObject);
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(
            IDbConnection connection, string query)
        {
            return await connection.QueryAsync<T>(query);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(
            IDbConnection connection, string query, object queryObject)
        {
            return await connection.QueryAsync<T>(
                query, queryObject);
        }        
    }
}
