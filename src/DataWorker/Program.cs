using DataRefiner;
using Domain.Interfaces;
using Infrastructure.Repository;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddScoped<IProcessRepository, ProcessRepository>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
