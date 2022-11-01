using Microsoft.Recognizers.Text;
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

        var results = controller.RecognizeCombined(new RecognizeCombinedInput()
        {
            Text =
                "The trip between six-mile and ten-mile to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.",
            Culture = "en-us",
            Entities = new List<string>() { "dimension-range" },
        });
        
        Assert.Collection<ModelResult>(results, 
            item => Assert.Equal(item.Text ,"between six-mile and ten-mile"),
            item => Assert.Equal(item.Text, "six-mile"),
            item => Assert.Equal(item.Text, "ten-mile"));
    }
    
    [Fact]
    public void TestRecognizeNumber()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        var results = controller.RecognizeNumber(new RecognizeInput()
        {
            Text = "I have two apples",
            Culture = "en-us"
        });
        
        Assert.Collection<ModelResult>(results, 
            item => Assert.Equal(item.Text ,"two"));
    }
    
    [Fact]
    public void TestRecognizeNumberRange()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        var results = controller.RecognizeNumberRange(new RecognizeInput()
        {
            Text = "the number is between 2 and 5",
            Culture = "en-us"
        });
        
        Assert.Collection<ModelResult>(results, 
            item => Assert.Equal(item.Text ,"between 2 and 5"));
    }
    
    [Fact]
    public void TestRecognizeCurrency()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        var results = controller.RecognizeCurrency(new RecognizeNumberWithUnitInput()
        {
            Text = "Interest expense in the 1988 third quarter was $ 75.3 million",
            Culture = "en-us"
        });
        
        Assert.Collection<ModelResult>(results, 
            item => Assert.Equal(item.Text ,"$ 75.3 million"));
    }
    
    [Fact]
    public void TestRecognizeCurrencyRange()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        var results = controller.RecognizeCurrencyRange(new RecognizeRangeWithUnitsInput()
        {
            Text = "Interest expense in the 1988 third quarter was between $ 75.3 million and $ 80 million",
            Culture = "en-us"
        });
        
        Assert.Collection<ModelResult>(results, 
            item => Assert.Equal(item.Text ,"between $ 75.3 million and $ 80 million"));
    }
    
    [Fact]
    public void TestRecognizeDimension()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        var results = controller.RecognizeDimension(new RecognizeRangeWithUnitsInput()
        {
            Text = "The six-mile trip to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.",
            Culture = "en-us"
        });
        
        Assert.Collection<ModelResult>(results, 
            item => Assert.Equal(item.Text ,"six-mile"));
    }
    
    [Fact]
    public void TestRecognizeDimensionRange()
    {
        var mockService = new Mock<RecognizeService>();

        var controller = new RecognizerController(mockService.Object);

        var results = controller.RecognizeDimensionRange(new RecognizeRangeWithUnitsInput()
        {
            Text = "The trip between six-mile and ten-mile to my airport hotel that had taken 20 minutes earlier in the day took more than three hours.",
            Culture = "en-us"
        });
        
        Assert.Collection<ModelResult>(results, 
            item => Assert.Equal(item.Text ,"between six-mile and ten-mile"));
    }
}