using Xunit;
using Moq;
using Recognizer.Controllers;
using Recognizer.Models;
using Recognizer.services;

namespace Recognizer.Test;

public class RecognizeControllerTest
{
    [Fact]
    public void TestRecognizeCombined()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        Assert.NotEmpty(controller.RecognizeCombined(new RecognizeCombinedInput()
        {
            Text = "The trip between six-mile and ten-mile to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.",
            Culture = "en-us",
            Entities = new List<string>(){"dimension-range"},
        }));
    }
    
    [Fact]
    public void TestRecognizeNumber()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        Assert.NotEmpty(controller.RecognizeNumber(new RecognizeInput()
        {
            Text = "I have two apples",
            Culture = "en-us"
        }));
    }
    
    [Fact]
    public void TestRecognizeNumberRange()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        Assert.NotEmpty(controller.RecognizeNumberRange(new RecognizeInput()
        {
            Text = "between 2 and 5",
            Culture = "en-us"
        }));
    }
    
    [Fact]
    public void TestRecognizeCurrency()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        Assert.NotEmpty(controller.RecognizeCurrency(new RecognizeNumberWithUnitInput()
        {
            Text = "Interest expense in the 1988 third quarter was $ 75.3 million",
            Culture = "en-us"
        }));
    }
    
    [Fact]
    public void TestRecognizeCurrencyRange()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        Assert.NotEmpty(controller.RecognizeCurrencyRange(new RecognizeRangeWithUnitsInput()
        {
            Text = "Interest expense in the 1988 third quarter was between $ 75.3 million and $ 80 million",
            Culture = "en-us"
        }));
    }
    
    [Fact]
    public void TestRecognizeDimension()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        Assert.NotEmpty(controller.RecognizeDimension(new RecognizeRangeWithUnitsInput()
        {
            Text = "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.",
            Culture = "en-us"
        }));
    }
    
    [Fact]
    public void TestRecognizeDimensionRange()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        Assert.NotEmpty(controller.RecognizeDimensionRange(new RecognizeRangeWithUnitsInput()
        {
            Text = "The trip between six-mile and ten-mile to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.",
            Culture = "en-us"
        }));
    }
}