using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Process
    {        
        public int Id {get; set;}
        public string NameFile {get; set;} = "";
        public Status Status {get; set;}
        public DateTime Start {get; set;}
        public DateTime End {get; set;}
    }

    public enum Status
    {
        AwaitingProcessing,
        Completed,
        Error
    }
}