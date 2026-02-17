# RefinedDataViewer-Parquet-POC
POC of a cloud-ready data system that visualizes and modifies large predictive datasets at an aggregated level while enforcing micro-level business rules. It minimizes SQL dependency by leveraging Parquet files and applies Event Sourcing (Delta-based changes) to ensure performance, scalability, and full traceability before consolidating updates at the granular SKU × Client level.

## Project Goal
This project is a POC (Proof of Concept) of a system designed to visualize and modify data at a macro level while enforcing and respecting micro-level business rules. It includes filtering capabilities and minimizes SQL dependency by storing most data as Parquet files, without sacrificing performance. The system receives .csv files as input.

The decision to minimize SQL usage and rely primarily on Parquet files is strategic. The architecture is designed with cloud deployment in mind, where SQL operations are generally more expensive. At the same time, performance remains a key requirement.

The project applies the ES (Event Sourcing) concept. Instead of updating records directly, Deltas (which also function as logs) are created for every change. Since the system handles large volumes of data, this approach reduces the amount of data written while maintaining full traceability.

The main objective is to visualize and modify large predictive datasets. The most granular level of data (Base Volume) is defined as SKU × Client, containing a value. A Catalog dataset provides additional SKU properties that enable filtering of the Base Volume.

Data is visualized and initially modified at a higher aggregation level (GroupKey, which functions similarly to a Brand, for example). Each change is stored as a Delta. Whenever the visualization function is executed again, all Deltas are applied dynamically.

Only when the process is finalized are the accumulated changes consolidated and written into a new Parquet file, with updates applied at the micro level (SKU × Client).

## Logic and Implementation

### Business Rules (Hierarchy Axioms)
To ensure data consistency, the Catalog must follow a strict one-to-many hierarchy:

1. GroupKey 1 : n Categories

2. Category 1 : n SKUs

*(In summary: GroupKey > Category > SKU)*

### Data Layers
The system operates across three distinct abstraction levels:

- Particle Level: The raw granularity of the Base Volume (SKU x Client). Changes are only committed here during the Consolidation Process.

- Minimum Grouping (Atomic Rule): The lowest level where a business rule or filter can be applied. It serves as the bridge between raw data and business logic.

- Maximum Grouping (Visualization): A heavily aggregated layer used for UI/Dashboarding and the format in which Deltas are recorded.


## How to Run

### Preparing to run the project
Make a file .env at the root folder with the line: 

`MSSQL_PASSWORD=PUT_YOUR_PASSWORD_HERE`

So that the docker will create the database with your password. Put that same password in the appsettings.json of the API.

### Running the project
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

### Creating files with python (optional)
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