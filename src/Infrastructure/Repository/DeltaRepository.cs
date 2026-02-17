using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository
{
    public class DeltaRepository (IConfiguration configuration) : IDeltaRepository
    {        
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");

        public async Task AddAsync (Delta delta)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string sql = "INSERT INTO Delta (GroupKey, Value, Filter, FilterValue, TimeStamp) VALUES (@GroupKey, @Value, @Filter, @FilterValue, GETDATE())";
            await db.ExecuteAsync(sql, delta);
        }
        public async Task<IEnumerable<Delta>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT * FROM Delta";
            return await connection.QueryAsync<Delta>(sql);
        }
    
    }
}