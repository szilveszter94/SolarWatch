using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Service.Processors;
using SolarWatch.Service.Providers;

namespace SolarWatchTest;

[TestFixture]
public class SolarWatchControllerTests
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<IWeatherDataProvider> _weatherDataProviderMock;
    private Mock<ILocationDataProvider> _locationDataProviderMock;
    private Mock<ILocationDataProcessor> _locationDataProcessorMock;
    private Mock<ISunsetSunriseDataProcessor> _sunsetSunriseDataProcessorMock;
    private SolarWatchController _controller;

    private const string City = "Budapest";
    private readonly DateTime _date = new DateTime(2022, 02, 02);

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _locationDataProcessorMock = new Mock<ILocationDataProcessor>();
        _sunsetSunriseDataProcessorMock= new Mock<ISunsetSunriseDataProcessor>();
        _weatherDataProviderMock = new Mock<IWeatherDataProvider>();
        _locationDataProviderMock = new Mock<ILocationDataProvider>();
        
        _controller = new SolarWatchController(_loggerMock.Object,
            _locationDataProcessorMock.Object, _sunsetSunriseDataProcessorMock.Object, _weatherDataProviderMock.Object,
            _locationDataProviderMock.Object);
    }
    
    [Test]
    public async Task GetSunsetSunriseReturnsNotFoundResultIfWeatherDataProviderFails()
    {
        // Arrange
        _locationDataProviderMock.Setup(x => x.GetCityLocationData(It.IsAny<string>())).Throws(new Exception());

        // Act
        var result = await _controller.GetSunsetSunrise(City, _date);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetSunsetSunriseReturnsNotFoundResultIfSunsetSunriseDataProviderFails()
    {
        // Arrange
        _weatherDataProviderMock
            .Setup(x => x.GetSunsetSunriseData(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<DateTime>()))
            .Throws(new Exception());
        // Act
        var result = await _controller.GetSunsetSunrise(City, _date);

        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

    [Test]
    public async Task GetSunsetSunriseReturnsNotFoundResultIfWeatherDataIsInvalid()
    {
        // Arrange
        var weatherData = "{}";
        _locationDataProviderMock.Setup(x => x.GetCityLocationData(It.IsAny<string>())).ReturnsAsync(weatherData);
        _locationDataProcessorMock.Setup(x => x.Process(weatherData)).Throws<Exception>();
    
        // Act
        var result = await _controller.GetSunsetSunrise(City, _date);
    
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task GetSunsetSunriseReturnsNotFoundResultIfSunsetSunriseDataIsInvalid()
    {
        // Arrange
        var sunsetSunriseData = "{}";
        _weatherDataProviderMock.Setup(x => x.GetSunsetSunriseData(It.IsAny<double>(), It.IsAny<double>(), _date)).ReturnsAsync(sunsetSunriseData);
        _sunsetSunriseDataProcessorMock.Setup(x => x.Process(sunsetSunriseData)).Throws<Exception>();
    
        // Act
        var result = await _controller.GetSunsetSunrise(City, _date);
    
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
}