using System.Net;
using Newtonsoft.Json;
using SolarWatch.Model;
using Xunit;
using Xunit.Abstractions;

namespace SolarWatchUnitTests.IntegrationTests;

public class SolarWatchControllerTests 
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public SolarWatchControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/SolarWatch/PopulateAutocompleteTable")]
    [InlineData("/SolarWatch/GetCitiesForAutoComplete?suggestion=Bud")]
    [InlineData("/SolarWatch/GetSunsetSunrise?city=Budapest&date=2024.03.31")]
    [InlineData("/SolarWatch/GetAllCityInformation")]
    [InlineData("/SolarWatch/GetCityInformationById/1")]
    [InlineData("/SolarWatch/GetAllLocationData")]
    [InlineData("/SolarWatch/GetLocationDataById/1")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetCitiesForAutocomplete?suggestion=Bud")]
    public async Task GetCitiesForAutocomplete_Endpoint_ReturnsSuccess(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var expectedResult = new AutocompleteCityModel() { City = "Budapest", Id = 1 };
        // Act
        var response = await client.GetAsync(url);

        // Assert
        List<AutocompleteCityModel> result = await ConvertResponseData<List<AutocompleteCityModel>>(response);
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        Assert.Equal(expectedResult, result[0]);
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetCitiesForAutocomplete?suggestion=WrongData")]
    public async Task GetCitiesForAutocomplete_Endpoint_ReturnsEmptyArrayNonExistingData(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var expectedResult = new List<AutocompleteCityModel>();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        List<AutocompleteCityModel> cities = await ConvertResponseData<List<AutocompleteCityModel>>(response);
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        Assert.Equal(expectedResult, cities);
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetSunsetSunrise?city=Budapest&date=2024.03.31")]
    public async Task GetSunsetSunriseData_Endpoint_ReturnsSuccess(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var expectedResult = new CityInformation()
            { City = "Budapest", Date = new DateTime(2024, 03, 31), Sunrise = "4 AM", Sunset = "6 PM" };

        // Act
        var response = await client.GetAsync(url);

        // Assert
        CityInformation cities = await ConvertResponseData<CityInformation>(response);
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        Assert.Equal(expectedResult, cities);
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetSunsetSunrise?city=fail&date=2024.03.31")]
    public async Task GetSunsetSunriseData_Endpoint_ReturnsNotFoundInvalidCity(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetSunsetSunrise?city=Budapest&date=invalid")]
    public async Task GetSunsetSunriseData_Endpoint_ReturnsBadRequestInvalidDate(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetAllCityInformation")]
    public async Task GetAllCityInformationReturnsValidData(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var expectedResult = new List<CityInformation>()
        {
            new CityInformation()
                { City = "Budapest", Id = 1, Date = new DateTime(2024, 03, 31), Sunrise = "4 AM", Sunset = "6 PM" }
        };
        // Act
        var response = await client.GetAsync(url);

        List<CityInformation> result = await ConvertResponseData<List<CityInformation>>(response);
        // Assert
        Assert.Equivalent(expectedResult, result);
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetAllLocationData")]
    public async Task GetAllLocationDataReturnsValidData(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var expectedResult = new List<LocationData>()
        {
            new () { City = "Budapest", Id = 1, Lat = 47, Lon = 21 }};
        // Act
        var response = await client.GetAsync(url);

        List<LocationData> result = await ConvertResponseData<List<LocationData>>(response);
        // Assert
        Assert.Equivalent(expectedResult, result);
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetCityInformationById/1")]
    public async Task GetCityInformationByIdReturnsValidData(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var expectedResult = new CityInformation()
                { City = "Budapest", Id = 1, Date = new DateTime(2024, 03, 31), Sunrise = "4 AM", Sunset = "6 PM" } ;
        
        // Act
        var response = await client.GetAsync(url);

        CityInformation result = await ConvertResponseData<CityInformation>(response);
        // Assert
        Assert.Equal(expectedResult, result);
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetCityInformationById/53")]
    public async Task GetCityInformationByIdReturnsNotFoundObjectIdNotValid(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync(url);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetLocationDataById/1")]
    public async Task GetLocationDataByIdReturnsValidData(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var expectedResult = new LocationData() { City = "Budapest", Id = 1, Lat = 47, Lon = 21 } ;
        
        // Act
        var response = await client.GetAsync(url);

        LocationData result = await ConvertResponseData<LocationData>(response);
        // Assert
        Assert.Equal(expectedResult, result);
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }
    
    [Theory]
    [InlineData("/SolarWatch/GetLocationDataById/53")]
    public async Task GetLocationDataByIdReturnsNotFoundObjectIdNotValid(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync(url);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    async Task<dynamic> ConvertResponseData<T>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
        var result = responseData.data;
        T converted = result.ToObject<T>();
        return converted;
    }
}