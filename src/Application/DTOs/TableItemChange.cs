namespace Application.DTOs
{
    public class TableItemChange
    {        
        public int GroupKey {get; set;}
        public int Category {get; set;} // Filter option
        public int SKU {get; set;} // Filter option
        public double OriginalSumValue {get; set;}
        public double ChangedSumValue {get; set;}
    }
}