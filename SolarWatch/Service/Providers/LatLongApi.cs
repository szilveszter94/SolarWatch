namespace SolarWatch.Service.Providers;

public class LatLongApi : ILocationDataProvider
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    
    public LatLongApi(ILogger<LatLongApi> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task<string> GetCityLocationData(string city)
    {
        var apiKey = _configuration["OPEN_WEATHER_API_KEY"];
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";
        using var client = new HttpClient();

        _logger.LogInformation($"Calling OpenWeather API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}