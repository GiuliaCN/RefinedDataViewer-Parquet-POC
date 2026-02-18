using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using DuckDB.NET.Data;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository
{
    public class HierarchySchemaRepository(IConfiguration configuration) : IHierarchySchemaRepository
    {
        private readonly string _fileHierarchySchema = configuration.GetValue<string>("FileSettings:HierarchySchemaParquet")
            ?? throw new InvalidOperationException("FilePath not configured");

        public async Task<IEnumerable<HierarchySchema>> GetAllAsync()
        {
            using var connection = new DuckDBConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var sql = $"SELECT * FROM read_parquet('{_fileHierarchySchema}')";
            IEnumerable<HierarchySchema> results = await connection.QueryAsync<HierarchySchema>(sql);
            return results;
        }
    }
}