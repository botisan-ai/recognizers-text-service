namespace Recognizer.Models
{
    public class RecognizeInput
    {
        public string Text { get; set; }
        public string Culture? { get; set; }
    }

    public class RecognizeNumberWithUnitInput : RecognizeInput
    {
        public List<string> Unit? { get; set; }
    }

    public class RecognizeRangeWithUnitsInput : RecognizeNumberWithUnitInput
    {
        public Boolean ShowNumbers? { get; set; }
    }    
    
    public class RecognizeCombinedInput: RecognizeRangeWithUnitsInput {
        public List<string> Entities? { get; set; }
        public Boolean MergeResults? { get; set; }
    }
}
