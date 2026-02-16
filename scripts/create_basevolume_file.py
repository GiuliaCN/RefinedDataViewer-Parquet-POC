import random
import csv
from pathlib import Path

def main():
    parent_dir = Path.cwd().parent
    data_dir = parent_dir / 'Data'
    
    csv_file_path = data_dir / 'basevolume.csv'
    data = CreateTable()
    
    with open(csv_file_path, mode='w', newline='') as file:
        writer = csv.writer(file)
        writer.writerows(data)
        
    print(f"CSV file '{csv_file_path}' created successfully.")


def CreateTable():
    groupKeyCount = random.randint(1,100)
    data = []
    for i in range(1,groupKeyCount+1):
        data += CreateSKU(i)
    data.insert(0,["SKU","Client","Value"])
    return data

def CreateSKU(id):
    clientCount = random.randint(200,1000)
    data = []
    for i in range(1,clientCount+1):
        data += [[id] + CreateClient(i)]
    return data
    
def CreateClient(id):
    data = [id,random.randint(1,1000)]
    return data

if __name__ == "__main__":
    main()