using System.Globalization;
using CsvHelper;
using Domain.Entities;
using Domain.Interfaces;
using Parquet.Serialization;

namespace DataRefiner;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _hierarchySchemaPath;
    private readonly string _atomicMatrixPath;
    private readonly string _processedDirectory;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _hierarchySchemaPath = configuration.GetValue<string>("FileSettings:HierarchySchemaPath") ?? "";
        _atomicMatrixPath = configuration.GetValue<string>("FileSettings:AtomicMatrixPath") ?? "";
        _processedDirectory = configuration.GetValue<string>("FileSettings:ProcessedFolder") ?? "";
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            try
            {
                using(IServiceScope scope = _serviceProvider.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IProcessRepository>;
                    var awatingProcessList = (await repository().GetAllAsync()).Where(x => x.Active);
                    foreach (Process process in awatingProcessList)
                    {
                        await repository().StartAsync(process);
                        if (process.Code == "ProcessFiles")
                        {
                            await ProcessHierarchySchema();
                            await ProcessAtomicMatrix();
                        }
                        await repository().EndAsync(process);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task ProcessHierarchySchema()
    {
        List<HierarchySchema> list = new();
        using (var reader = new StreamReader(_hierarchySchemaPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<HierarchySchema>();
            foreach (var x in records) list.Add(x);
        }
        await ParquetSerializer.SerializeAsync(list, _processedDirectory + "/hierarchy_schema.parquet");
    }
    private async Task ProcessAtomicMatrix()
    {
        List<AtomicMatrix> list = new();
        using (var reader = new StreamReader(_atomicMatrixPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {            
            var records = csv.GetRecords<AtomicMatrix>();
            foreach (var x in records) list.Add(x);
        }
        await ParquetSerializer.SerializeAsync(list, _processedDirectory + "/atomic_matrix.parquet");
    }
}
