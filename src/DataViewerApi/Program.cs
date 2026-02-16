using Dapper;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found");

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

EnsureDatabaseCreated (connectionString);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


static void EnsureDatabaseCreated(string connectionString)
{
    using var connection = new SqlConnection(connectionString.Replace("Database=DataViewerDB;", "Database=master;"));

    connection.Execute("IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'DataViewerDB') CREATE DATABASE DataViewerDB;");

    using var dbConnection = new SqlConnection(connectionString);
    
    dbConnection.Execute(@"
        IF NOT EXISTS (SELECT * FROM sys.table_objects WHERE name = 'Process')
        CREATE TABLE Process (
            Id INT PRIMARY KEY IDENTITY,
            NameFile VARCHAR(255),
            Status VARCHAR(50),
            Start DATETIME DEFAULT GETDATE(),
            End DATETIME DEFAULT GETDATE()
        )");

    dbConnection.Execute(@"
        IF NOT EXISTS (SELECT * FROM sys.table_objects WHERE name = 'Delta')
        CREATE TABLE Delta (
            Id INT PRIMARY KEY IDENTITY,
            GroupKeyId INT,
            Value INT,
            Filter VARCHAR(255),
            TimeStamp DATETIME DEFAULT GETDATE()
        )");
}