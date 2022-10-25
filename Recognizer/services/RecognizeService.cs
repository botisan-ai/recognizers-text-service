using Microsoft.Recognizers.Text;
using Recognizer.Models;

namespace Recognizer.services;

public class RecognizeService
{
    public List<ModelResult> filterModelResultsByUnit(List<ModelResult> modelResults, List<string> units)
    {
        return modelResults.Where(modelResult => {
            if(units == null || units.Count == 0)
            {
                return true;
            }
            return units.Contains(modelResult.Resolution["unit"].ToString());
        }).ToList();
    }
}