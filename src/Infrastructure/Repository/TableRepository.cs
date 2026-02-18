using Application.DTOs;
using Application.Interfaces;
using Dapper;
using DuckDB.NET.Data;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Repository
{
    public class TableRepository(IConfiguration configuration) : ITableRepository
    {
        
        private readonly string _fileHierarchySchema = configuration.GetValue<string>("FileSettings:HierarchySchemaParquet")
            ?? throw new InvalidOperationException("FilePath not configured");
        private readonly string _fileAtomicMatrix = configuration.GetValue<string>("FileSettings:AtomicMatrixParquet")
            ?? throw new InvalidOperationException("FilePath not configured");

        public async Task<IEnumerable<TableItemChange>> GetTableItemChangeAsync()
        {            
            using var connection = new DuckDBConnection("DataSource=:memory:");
            await connection.OpenAsync();

            var sql = @$"
                SELECT ParentNode,IntermediateNode,AM.AtomicEntity
                ,SUM(AM.Value) AS OriginalSumValue
                ,SUM(AM.Value) AS ChangedSumValue 
                FROM read_parquet('{_fileAtomicMatrix}') AM
                JOIN read_parquet('{_fileHierarchySchema}') HS ON AM.AtomicEntity = HS.AtomicEntity
                GROUP BY ParentNode,IntermediateNode,AM.AtomicEntity";
            IEnumerable<TableItemChange> results = await connection.QueryAsync<TableItemChange>(sql);
            return results;
        }
    }
}