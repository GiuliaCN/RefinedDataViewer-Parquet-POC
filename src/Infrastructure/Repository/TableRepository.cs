using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Dapper;
using DuckDB.NET.Data;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository
{
    public class TableRepository(IConfiguration configuration) : ITableRepository
    {
        
        private readonly string _fileCatalog = configuration.GetValue<string>("FileSettings:CatalogParquet")
            ?? throw new InvalidOperationException("FilePath not configured");
        private readonly string _fileBaseVolume = configuration.GetValue<string>("FileSettings:BaseVolumeParquet")
            ?? throw new InvalidOperationException("FilePath not configured");

        public async Task<IEnumerable<TableItemChange>> GetTableItemChangeAsync()
        {            
            using var connection = new DuckDBConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var sql = @$"
                SELECT GroupKey,Category,BS.SKU
                ,SUM(BS.Value) AS OriginalSumValue
                ,SUM(BS.Value) AS ChangedSumValue 
                FROM read_parquet('{_fileBaseVolume}') BS
                JOIN read_parquet('{_fileCatalog}') C ON BS.SKU = C.SKU
                GROUP BY GroupKey,Category,BS.SKU";
            IEnumerable<TableItemChange> results = await connection.QueryAsync<TableItemChange>(sql);
            return results;
        }
    }
}