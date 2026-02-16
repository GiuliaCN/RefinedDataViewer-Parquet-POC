# RefinedDataViewer-Parquet-POC
This repository is a Proof of Concept (POC) for a distributed data processing pipeline built with .NET 8. The project demonstrates how to handle large datasets by offloading heavy processing from a Web API to a background Worker Service, utilizing SQL Server for state management and Parquet for optimized storage.

## Preparing to run the project
Make a file .env at the root folder with the line: 

`MSSQL_PASSWORD=PUT_YOUR_PASSWORD_HERE`

So that the docker will create the database with your password. Put that same password in the appsettings.json of the API.

## Running the project
Open at terminal at the root folder and run the command to inniciate the docker with the SQL:
```
docker-compose up -d
```

To run the Api:
```
cd src/MinhaApi
dotnet build
```

To run the Worker:
```
cd ../MeuWorker
dotnet build
```

## Creating files (optional)
You can create files to use in the application with the .py scripts in the scripts folder.

They will create a folder 'Data' if it doesn't exist and .csv files will be there.

To create catalog.csv:
```
cd ./scripts
python .\create_catalog_file.py
```

To create basevolume.csv:
```
cd ./scripts
python .\create_basevolume_file.py
```