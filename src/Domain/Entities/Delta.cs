namespace Domain.Entities
{
    public class Delta
    {
        public int Id {get; set;}
        public int GroupKey {get; set;}
        public int Value {get; set;}
        public string Filter {get; set;} = "";
        public DateTime TimeStamp {get; set;}
    }
}