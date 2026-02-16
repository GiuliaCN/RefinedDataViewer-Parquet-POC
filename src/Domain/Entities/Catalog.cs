using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{    
    // Input file and Parquet file
    public class Catalog
    {
        public int SKU {get; set;} // Filter option
        public int Category {get; set;} // Filter option
        public int GroupKeyId {get; set;}
    }
}