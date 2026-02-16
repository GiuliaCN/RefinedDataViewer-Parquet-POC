using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    // Input file and Parquet file
    public class BaseVolume
    {
        public int SKU {get; set;}
        public int Client {get; set;}
        public int Value {get; set;}
    }
}