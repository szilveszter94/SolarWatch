using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Model;
using SolarWatch.Repository;
using SolarWatch.Service.Processors;
using SolarWatch.Service.Providers;

namespace SolarWatchTest;

[TestFixture]
public class SolarWatchControllerTests
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private Mock<ILocationDataProvider> _locationDataProviderMock;
    private Mock<IWeatherDataProvider> _weatherDataProviderMock;
    private Mock<ILocationDataProcessor> _locationDataProcessorMock;
    private Mock<ISunsetSunriseDataProcessor> _sunsetSunriseDataProcessorMock;
    private Mock<ICityRepository> _cityRepositoryMock;
    private SolarWatchController _controller;

    private const string City = "Budapest";
    private readonly DateTime _date = new DateTime(2022, 02, 02);

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _locationDataProviderMock = new Mock<ILocationDataProvider>();
        _weatherDataProviderMock = new Mock<IWeatherDataProvider>();
        _locationDataProcessorMock = new Mock<ILocationDataProcessor>();
        _sunsetSunriseDataProcessorMock= new Mock<ISunsetSunriseDataProcessor>();
        _cityRepositoryMock = new Mock<ICityRepository>();
        _controller = new SolarWatchController(_loggerMock.Object, _locationDataProcessorMock.Object,
            _sunsetSunriseDataProcessorMock.Object, _weatherDataProviderMock.Object,
            _locationDataProviderMock.Object, _cityRepositoryMock.Object);
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
    
    [Test]
    public async Task GetSunsetSunriseReturnsOkResultIfWeatherDataIsValid()
    {
        // Arrange
        string sunrise = "6 PM", sunset = "5 AM", weatherData = "{}";
        
        var expectedLocationData = new LocationData { City = City, Lat = 100, Lon = -50 };
        var expectedSunriseSunsetData = new SunsetSunriseData(sunrise, sunset);
        var expectedFinalResult = new CityInformation { City = City, Date = _date, Sunrise = sunrise,Sunset = sunset};
        _cityRepositoryMock.Setup((x) => x.GetLocationDataByCity(It.IsAny<string>())).ReturnsAsync((LocationData)null);
        _cityRepositoryMock.Setup((x) => x.GetCityByNameAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync((CityInformation)null);
        _locationDataProviderMock.Setup(x => x.GetCityLocationData(It.IsAny<string>())).ReturnsAsync(weatherData);
        _locationDataProcessorMock.Setup(x => x.Process(weatherData)).Returns(expectedLocationData);
        _weatherDataProviderMock
            .Setup(x => x.GetSunsetSunriseData(It.IsAny<double>(), It.IsAny<double>(), _date)).ReturnsAsync(weatherData);
        _sunsetSunriseDataProcessorMock.Setup(x => x.Process(weatherData)).Returns(expectedSunriseSunsetData);
        // Act
        var result = await _controller.GetSunsetSunrise(City, _date);
        // Assert
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        var res = ((OkObjectResult)result.Result).Value as CityInformation;
        Assert.That(res, Is.EqualTo(expectedFinalResult));
    }
    
    [Test]
    public async Task GetSunsetSunriseReturnsOkResultIfCityRepositoryHasData()
    {
        // Arrange
        string sunrise = "6 PM", sunset = "5 AM";
        var locationData = new LocationData {City = City, Lat = 1, Lon = 1};
        var expectedFinalResult = new CityInformation { City = City, Date = _date, Sunrise = sunrise,Sunset = sunset};
        _cityRepositoryMock.Setup((x) => x.GetLocationDataByCity(It.IsAny<string>())).ReturnsAsync(locationData);
        _cityRepositoryMock.Setup((x) => x.GetCityByNameAndDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(expectedFinalResult);
        
        // Act
        var result = await _controller.GetSunsetSunrise(City, _date);
        // Assert
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        var res = ((OkObjectResult)result.Result).Value as CityInformation;
        Assert.That(res, Is.EqualTo(expectedFinalResult));
    }
}