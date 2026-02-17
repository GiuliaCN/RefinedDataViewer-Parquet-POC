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
    private readonly string _catalogPath;
    private readonly string _basePath;
    private readonly string _processedDirectory;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _catalogPath = configuration.GetValue<string>("FileSettings:CatalogPath") ?? "";
        _basePath = configuration.GetValue<string>("FileSettings:BaseVolumePath") ?? "";
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
                            await ProcessCatalog();
                            await ProcessBaseVolume();
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

    private async Task ProcessCatalog()
    {
        List<Catalog> list = new();
        using (var reader = new StreamReader(_catalogPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Catalog>();
            foreach (var x in records) list.Add(x);
        }
        await ParquetSerializer.SerializeAsync(list, _processedDirectory + "/catalog.parquet");
    }
    private async Task ProcessBaseVolume()
    {
        List<BaseVolume> list = new();
        using (var reader = new StreamReader(_basePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {            
            var records = csv.GetRecords<BaseVolume>();
            foreach (var x in records) list.Add(x);
        }
        await ParquetSerializer.SerializeAsync(list, _processedDirectory + "/basevolume.parquet");
    }
}
