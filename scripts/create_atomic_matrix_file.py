import random
import csv
from pathlib import Path
import os

def main():
    parent_dir = Path.cwd().parent
    data_dir = parent_dir / 'Data'
    os.makedirs(data_dir, exist_ok=True)
    
    csv_file_path = data_dir / 'atomic_matrix.csv'
    data = CreateTable()
    
    with open(csv_file_path, mode='w', newline='') as file:
        writer = csv.writer(file)
        writer.writerows(data)
        
    print(f"CSV file '{csv_file_path}' created successfully.")


def CreateTable():
    atomicEntityCount = 100
    data = []
    for i in range(1,atomicEntityCount+1):
        data += CreateAtomicEntity(i)
    data.insert(0,["AtomicEntity","TargetEntity","Value"])
    return data

def CreateAtomicEntity(id):
    targetEntityCount = random.randint(200,1000)
    data = []
    for i in range(1,targetEntityCount+1):
        data += [[id] + CreateTargetEntity(i)]
    return data
    
def CreateTargetEntity(id):
    data = [id,random.randint(1,1000)]
    return data

if __name__ == "__main__":
    main()