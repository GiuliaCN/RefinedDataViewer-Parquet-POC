import random
import csv
from pathlib import Path
import os

def main():
    parent_dir = Path.cwd().parent
    data_dir = parent_dir / 'Data'
    os.makedirs(data_dir, exist_ok=True)
    
    csv_file_path = data_dir / 'hierarchy_schema.csv'
    data = CreateTable()
    
    with open(csv_file_path, mode='w', newline='') as file:
        writer = csv.writer(file)
        writer.writerows(data)
        
    print(f"CSV file '{csv_file_path}' created successfully.")


def CreateTable():
    parentNodeCount = random.randint(1,100)
    data = []
    for i in range(1,parentNodeCount+1):
        data += CreateParentNode(i)
    data.insert(0,["ParentNode","IntermediateNode","AtomicEntity"])
    return data

def CreateParentNode(id):
    intermediateNodeCount = random.randint(1,100)
    data = []
    for i in range(1,intermediateNodeCount+1):
        data += [[id] + row for row in CreateIntermediateNode(i)]
    return data
    
def CreateIntermediateNode(id):
    atomicEntityCount = 100
    data = [[id,i] for i in range(1,atomicEntityCount+1)]
    return data

if __name__ == "__main__":
    main()