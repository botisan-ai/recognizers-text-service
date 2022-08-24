namespace Recognizer.Models
{
    public class RecognizeInput
    {
        public string Text { get; set; }
        public string Culture { get; set; }
    }

    public class RecognizeNumberWithUnitInput : RecognizeInput
    {
        public List<string> Unit { get; set; }
    }

    public class RecognizeRangeWithUnitsInput : RecognizeNumberWithUnitInput
    {
        private bool ShowNumbers { get; set; }
    }    
}