using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Process
    {        
        public int Id {get; set;}
        public string Code {get; set;} = "ProcessFiles";
        public bool Active {get; set;} = true;
        public DateTime StartDate {get; set;} = new DateTime(1900,1,1);
        public DateTime EndDate {get; set;} = new DateTime(1900,1,1);
    }
}