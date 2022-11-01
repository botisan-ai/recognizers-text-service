using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Recognizer.Models;

namespace Recognizer.utils;

public class RangeUtil
{
    public static List<ModelResult> RecognizeRangeWithUnits(
        string rangeModelType, 
        List<ModelResult> srcModelResults,
        RecognizeRangeWithUnitsInput input)
    {
        var originalText = input.Text;
        var text = originalText;
        var culture = input.Culture;
        var showNumbers = input.ShowNumbers;
        
        if (srcModelResults == null || srcModelResults.Count == 0) {
            // return empty list if no currency found
            return new List<ModelResult>();
        }
        
        var offset = 0;
        var convertedNumberResult = new List<ModelResult>();
        
        // replace the currency occurrences to numbers
        foreach (var result in srcModelResults)
        {
            var value = new object();
            var hasValue = result.Resolution.TryGetValue("value", out value);
            if (!hasValue) {
                continue;
            }
            
            var replacement = value.ToString();
            String newText = text.Substring(0, offset + result.Start) 
                             + replacement 
                             + ((offset + result.End + 1 < text.Length) ? text.Substring(offset + result.End + 1) : "");
            
            var newStart = offset + result.Start;
            
            offset += replacement.Length - result.Text.Length;

            int newEnd = offset + result.End;
            text = newText;

            var newResolution = new SortedDictionary<string, object>(result.Resolution);
            newResolution.Add("origText", result.Text);
            newResolution.Add("origStart", result.Start);
            newResolution.Add("origEnd", result.End);

            convertedNumberResult.Add(new ModelResult {
                End = newEnd,
                Resolution = newResolution,
                Start = newStart,
                Text = replacement,
                TypeName = result.TypeName,
            });
        }

        var rangeModelResult = NumberRecognizer.RecognizeNumberRange(text, culture);

        var convertedRangeResult = new List<ModelResult>();

        foreach (var rangeResult in rangeModelResult) {
            // internal offset to update the individual rangeResult
            var internalOffset = 0;
            var mergeCount = 0;
            // re-use text
            var newText = rangeResult.Text;
            var units = new List<string>();

            foreach (var numberResult in convertedNumberResult) {
                if (numberResult.Start < rangeResult.Start || numberResult.End > rangeResult.End) {
                    continue;
                }

                var replacementObj = new object();
                numberResult.Resolution.TryGetValue("origText", out replacementObj);
                var replacement = replacementObj.ToString();
                
                var firstEnd = internalOffset + numberResult.Start - rangeResult.Start;
                var lastStart = internalOffset + numberResult.End - rangeResult.Start + 1;
                newText = newText.Substring(0, firstEnd) 
                          + replacement + ((lastStart < newText.Length) ? newText.Substring(lastStart) : "");
                internalOffset += replacement.Length - numberResult.Text.Length;

                var unitObj = new object();
                numberResult.Resolution.TryGetValue("unit", out unitObj);
                units.Add(unitObj.ToString());
                mergeCount += 1;
            }

            if (mergeCount <= 0) {
                continue;
            }

            // calculate the start and end index
            int newStart = originalText.IndexOf(newText);
            int newEnd = newStart + newText.Length - 1;

            var newResolution = new SortedDictionary<string, object>(rangeResult.Resolution);
            newResolution.Add("unit", units);
            if (showNumbers) {
                newResolution.Add(
                        "numbers",
                        srcModelResults.FindAll(item => item.Start >= newStart && item.End <= newEnd)
                );
            }

            ModelResult newRangeResult = new ModelResult
            {
                Text = newText,
                Start = newStart,
                End = newEnd,
                TypeName = rangeModelType,
                Resolution = newResolution,
            };
            convertedRangeResult.Add(newRangeResult);
        }

        return convertedRangeResult;
    }
    
}