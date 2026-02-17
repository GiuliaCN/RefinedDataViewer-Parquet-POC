using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using DuckDB.NET.Data;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository
{
    public class CatalogRepository(IConfiguration configuration) : ICatalogRepository
    {
        private readonly string _fileCatalog = configuration.GetValue<string>("FileSettings:CatalogParquet")
            ?? throw new InvalidOperationException("FilePath not configured");

        public async Task<IEnumerable<Catalog>> GetAllAsync()
        {
            using var connection = new DuckDBConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var sql = $"SELECT * FROM read_parquet('{_fileCatalog}')";
            IEnumerable<Catalog> results = await connection.QueryAsync<Catalog>(sql);
            return results;
        }
    }
}