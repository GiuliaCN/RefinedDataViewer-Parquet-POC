import random
import csv
from pathlib import Path

def main():
    parent_dir = Path.cwd().parent
    data_dir = parent_dir / 'Data'
    
    csv_file_path = data_dir / 'catalog.csv'
    data = CreateTable()
    
    with open(csv_file_path, mode='w', newline='') as file:
        writer = csv.writer(file)
        writer.writerows(data)
        
    print(f"CSV file '{csv_file_path}' created successfully.")


def CreateTable():
    groupKeyCount = random.randint(1,100)
    data = []
    for i in range(1,groupKeyCount+1):
        data += CreateGroupKey(i)
    data.insert(0,["GroupKey","Category","SKU"])
    return data

def CreateGroupKey(id):
    categoryCount = random.randint(1,100)
    data = []
    for i in range(1,categoryCount+1):
        data += [[id] + row for row in CreateCategory(i)]
    return data
    
def CreateCategory(id):
    skuCount = 100
    data = [[id,i] for i in range(1,skuCount+1)]
    return data

if __name__ == "__main__":
    main()