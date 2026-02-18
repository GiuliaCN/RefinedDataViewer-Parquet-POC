# Hierarchical Data Adjustment Engine - POC
A scalable hierarchical data reconciliation engine that separates visualization, rule application, and atomic consolidation using a delta-driven architecture.

## Overview

This project is a Proof of Concept (POC) for a scalable hierarchical data adjustment engine designed to handle large datasets in a cloud-efficient manner.

The system enables macro-level adjustments over aggregated data while guaranteeing deterministic propagation to atomic-level records.

It prioritizes:

- Scalability
- Traceability
- Cost-efficient cloud deployment
- Clear separation of data abstraction layers

## Architectural Principles

### Event Sourcing-Based Mutation

Instead of mutating datasets directly, every adjustment generates a Delta event.

This ensures:

- Full change traceability
- Reduced write amplification
- Deterministic recomputation
- Safer concurrent modifications

Atomic data is only rewritten during a controlled consolidation phase.

### Parquet-First Storage Strategy

The system minimizes SQL dependency by persisting large datasets as Parquet files, reducing:

- Cloud storage cost
- Query engine dependency
- Infrastructure overhead

SQL is reserved only for lightweight orchestration tasks when necessary.

### Hierarchical Determinism

The engine enforces strict one-to-many hierarchical axioms:

*Parent Node > Intermediate Node > Atomic Entity*

This guarantees predictable propagation of macro-level adjustments to atomic records.

## Data Abstraction Layers

1. Atomic Layer: 
The most granular dataset representation.
Permanent changes are applied only during consolidation.

2. Rule Layer: 
Lowest grouping level where business rules and filters operate.

3. Visualization Layer: 
Highest aggregation level used for user interaction.
All adjustments are recorded here as Delta events.

## Processing Flow

1. Load atomic dataset from Parquet
2. Apply metadata hierarchy
3. Aggregate for visualization
4. Record adjustments as Delta events
5. Recompute views dynamically
6. Consolidate and persist atomic updates

## Core Focus

This project explores:

- Hierarchical state management
- Event-driven mutation models
- Large-scale dataset manipulation
- Cloud cost-aware architecture
- Separation between visualization state and persistent state


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
cd src/DataViewerApi
dotnet build
```

To run the Worker:
```
cd ../DataWorker
dotnet build
```

### Creating files with python (optional)
You can create files to use in the application with the .py scripts in the scripts folder.

They will create a folder 'Data' if it doesn't exist and .csv files will be there.

To create hierarchy_schema.csv:
```
cd ./scripts
python .\create_hierarchy_schema_file.py
```

To create atomic_matrix.csv:
```
cd ./scripts
python .\create_atomic_matrix_file.py
```