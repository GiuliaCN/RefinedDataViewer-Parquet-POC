using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{    
    // Input file and Parquet file
    public class HierarchySchema
    {
        public int ParentNode {get; set;} // Group by
        public int IntermediateNode {get; set;} // Filter option
        public int AtomicEntity {get; set;} // Filter option
    }
}