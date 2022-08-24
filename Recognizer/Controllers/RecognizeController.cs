using Microsoft.AspNetCore.Mvc;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.NumberWithUnit;
using Recognizer.Models;

namespace Recognizer.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class RecognizerController: ControllerBase
    {
        [HttpPost("combined")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ModelResult>> RecognizeCombined([FromBody] RecognizeInput input)
        {
            throw new NotImplementedException();
        }

        [HttpPost("number")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ModelResult>> RecognizeNumber([FromBody] RecognizeInput input)
        {
            var culture = input.Culture ?? Culture.English;
            return Ok(NumberRecognizer.RecognizeNumber(input.Text, culture));
        }

        [HttpPost("number_range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ModelResult>> RecognizeNumberRange([FromBody] RecognizeInput input)
        {
            var culture = input.Culture ?? Culture.English;
            return Ok(NumberRecognizer.RecognizeNumberRange(input.Text, culture));
        }

        [HttpPost("currency")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ModelResult>> RecognizeCurrency([FromBody] RecognizeNumberWithUnitInput input)
        {
            var result = FilterModelResultsByUnit(
                NumberWithUnitRecognizer.RecognizeCurrency(
                    input.Text, input.Culture ?? Culture.English),
                input.Unit
            );
            return Ok(result);
        }

        [HttpPost("dimension")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ModelResult>> RecognizeDimension([FromBody] RecognizeNumberWithUnitInput input)
        {
            var result = FilterModelResultsByUnit(
                NumberWithUnitRecognizer.RecognizeDimension(
                    input.Text, input.Culture ?? Culture.English),
                input.Unit
            );
            return Ok(result);
        }

        [HttpPost("currency_range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ModelResult>> RecognizeCurrencyRange([FromBody] RecognizeRangeWithUnitsInput input)
        {
            throw new NotImplementedException();
        }

        [HttpPost("dimension_range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<ModelResult>> RecognizeDimensionRange([FromBody] RecognizeRangeWithUnitsInput input)
        {
            throw new NotImplementedException();
        }

        private static List<ModelResult> FilterModelResultsByUnit(List<ModelResult> modelResults, List<string> units)
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
}