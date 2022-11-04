using Microsoft.AspNetCore.Mvc;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Microsoft.Recognizers.Text.DateTime;
using Microsoft.Recognizers.Text.Sequence;
using Recognizer.Models;
using Recognizer.services;
using Recognizer.utils;

namespace Recognizer.Controllers;

[ApiController]
[Route("recognize")]
public class RecognizerController: ControllerBase
{

    private readonly RecognizeService _recognizeService;

    public RecognizerController(RecognizeService recognizeService)
    {
        _recognizeService = recognizeService;
    }

    /**
     * recognize all number, currency, dimension and their range
     */
    [HttpPost("combined")]
    public List<ModelResult> RecognizeCombined([FromBody] RecognizeCombinedInput input)
    {
        return doRecognizeCombined(input);
    }

    /**
     * recognize number
     */
    [HttpPost("number")]
    public List<ModelResult> RecognizeNumber([FromBody] RecognizeInput input)
    {
        return NumberRecognizer.RecognizeNumber(input.Text, input.Culture);
    }

    /**
     * recognize number range
     */
    [HttpPost("number_range")]
    public List<ModelResult> RecognizeNumberRange([FromBody] RecognizeInput input)
    {
        return NumberRecognizer.RecognizeNumberRange(input.Text, input.Culture);
    }

    /**
     * recognize currency
     */
    [HttpPost("currency")]
    public List<ModelResult> RecognizeCurrency([FromBody] RecognizeNumberWithUnitInput input)
    {
        var result = _recognizeService.filterModelResultsByUnit(
            NumberWithUnitRecognizer.RecognizeCurrency(
                input.Text, input.Culture),
            input.Unit
        );
        return result;
    }

    /**
     * recognize currency_range
     */
    [HttpPost("currency_range")]
    public List<ModelResult> RecognizeCurrencyRange([FromBody] RecognizeRangeWithUnitsInput input)
    {
        var modelResults = RecognizeCurrency(input);
        return RangeUtil.RecognizeRangeWithUnits(
            "currency-range",
            modelResults,
            input
        );
    }

    /**
     * recognize dimension
     */
    [HttpPost("dimension")]
    public List<ModelResult> RecognizeDimension([FromBody] RecognizeNumberWithUnitInput input)
    {
        var result = _recognizeService.filterModelResultsByUnit(
            NumberWithUnitRecognizer.RecognizeDimension(
                input.Text, input.Culture),
            input.Unit
        );
        return result;
    }

    /**
     * recognize dimension range
     */
    [HttpPost("dimension_range")]
    public List<ModelResult> RecognizeDimensionRange([FromBody] RecognizeRangeWithUnitsInput input)
    {
        var modelResults = RecognizeDimension(input);
        return RangeUtil.RecognizeRangeWithUnits(
            "dimension-range",
            modelResults,
            input
        );
    }

    /**
     * recognize datetime
     */
    [HttpPost("datetime")]
    public List<ModelResult> RecognizeDateTime([FromBody] RecognizeInput input)
    {
        // TODO: make refTime configurable
        return DateTimeRecognizer.RecognizeDateTime(
            input.Text,
            input.Culture
        );
    }

    /**
     * recognize phone number
     */
    [HttpPost("phone_number")]
    public List<ModelResult> RecognizePhoneNumber([FromBody] RecognizeInput input)
    {
        return SequenceRecognizer.RecognizePhoneNumber(
            input.Text,
            input.Culture
        );
    }

    /**
     * method
     */
    private List<ModelResult> doRecognizeCombined(RecognizeCombinedInput input)
    {
        var entities = input.Entities;

        var numberResults = new List<ModelResult>();
        var currencyResults = new List<ModelResult>();
        var dimensionResults = new List<ModelResult>();

        var numberRangeResults = new List<ModelResult>();
        var currencyRangeResults = new List<ModelResult>();
        var dimensionRangeResults = new List<ModelResult>();

        var datetimeResults = new List<ModelResult>();
        var phoneNumberResults = new List<ModelResult>();


        if (entities == null || entities.Contains("number")) {
            numberResults.AddRange(RecognizeNumber(input));
        }

        if (entities == null || entities.Contains("currency") || entities.Contains("currency-range")) {
            currencyResults.AddRange(RecognizeCurrency(input));
        }

        if (entities == null || entities.Contains("dimension") || entities.Contains("dimension-range")) {
            dimensionResults.AddRange(RecognizeDimension(input));
        }

        if (entities == null || entities.Contains("numberrange")) {
            numberRangeResults.AddRange(RecognizeNumberRange(input));
        }

        if (entities == null || entities.Contains("currency-range")) {
            currencyRangeResults.AddRange(RangeUtil.RecognizeRangeWithUnits(
                    "currency-range",
                    currencyResults,
                    input
            ));
        }

        if (entities == null || entities.Contains("dimension-range")) {
            dimensionRangeResults.AddRange(RangeUtil.RecognizeRangeWithUnits(
                    "dimension-range",
                    dimensionResults,
                    input
            ));
        }

        if (entities == null || entities.Any(entity => entity.StartsWith("datetime"))) {
            datetimeResults.AddRange(RecognizeDateTime(input));
        }

        if (entities == null || entities.Contains("phonenumber")) {
            phoneNumberResults.AddRange(RecognizePhoneNumber(input));
        }

        var finalResults = new List<ModelResult>()
                .Concat(currencyRangeResults)
                .Concat(dimensionRangeResults)
                .Concat(numberRangeResults)
                .Concat(currencyResults)
                .Concat(dimensionResults)
                .Concat(numberResults)
                .Concat(datetimeResults)
                .Concat(phoneNumberResults)
                .Aggregate(
                        new List<ModelResult>(),
                        (results, modelResult) => {
                            if (!input.MergeResults) {
                                results.Add(modelResult);
                                return results;
                            }

                            var containsResult = results
                                    .Any(result => modelResult.Start >= result.Start
                                                   && modelResult.End <= result.End);

                            if (!containsResult) {
                                results.Add(modelResult);
                            }

                            return results;
                        }
                );
        return finalResults;
    }
}
