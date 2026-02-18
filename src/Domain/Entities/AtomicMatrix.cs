using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Input file and Parquet file
    public class AtomicMatrix
    {
        public int AtomicEntity {get; set;}
        public int TargetEntity {get; set;}
        public double Value {get; set;}
    }
}