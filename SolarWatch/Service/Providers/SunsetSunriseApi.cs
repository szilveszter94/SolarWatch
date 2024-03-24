namespace SolarWatch.Service.Providers;

public class SunsetSunriseApi : IWeatherDataProvider
{
    private readonly ILogger _logger;
    
    public SunsetSunriseApi(ILogger<SunsetSunriseApi> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetSunsetSunriseData(double lat, double lon, DateTime date)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&tzid=Europe/Budapest&date={date:yyyy-MM-dd}";
        using var client = new HttpClient();

        _logger.LogInformation($"Calling OpenWeather API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}