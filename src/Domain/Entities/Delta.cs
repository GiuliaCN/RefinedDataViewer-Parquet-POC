using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Delta
    {
        public int Id {get; set;}
        public int GroupKeyId {get; set;}
        public int Value {get; set;}
        public string Filter {get; set;} = "";
        public DateTime TimeStamp {get; set;}
    }
}