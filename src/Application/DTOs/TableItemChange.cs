namespace Application.DTOs
{
    public class TableItemChange
    {        
        public int ParentNode {get; set;} // Group by
        public int IntermediateNode {get; set;} // Filter option
        public int AtomicEntity {get; set;} // Filter option
        public double OriginalSumValue {get; set;}
        public double ChangedSumValue {get; set;}
    }
}