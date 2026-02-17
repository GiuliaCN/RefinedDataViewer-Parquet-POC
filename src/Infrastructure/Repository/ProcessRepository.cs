using System.Data;
using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository
{
    public class ProcessRepository(IConfiguration configuration) : IProcessRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found");

        public async Task AddAsync (Process process)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string sql = "INSERT INTO Process (Code, Active, StartDate, EndDate) VALUES (@Code, @Active, @StartDate, @EndDate)";
            await db.ExecuteAsync(sql, process);
        }
        public async Task StartAsync (Process process)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string sql = "UPDATE Process SET StartDate = GETDATE() WHERE Id = @Id";
            await db.ExecuteAsync(sql, process);
        }
        public async Task EndAsync (Process process)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string sql = "UPDATE Process SET EndDate = GETDATE(), Active = 0 WHERE Id = @Id";
            await db.ExecuteAsync(sql, process);
        }

        public async Task<IEnumerable<Process>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = "SELECT * FROM Process";
            return await connection.QueryAsync<Process>(sql);
        }
    }
}