using DotNetEnv;

namespace SolarWatch.Service.Providers;

public class LatLongApi : ILocationDataProvider
{
    private readonly ILogger _logger;
    
    public LatLongApi(ILogger<LatLongApi> logger)
    {
        _logger = logger;
    }
    
    public async Task<string> GetCityLocationData(string city)
    {
        Env.Load();
        var apiKey = Environment.GetEnvironmentVariable("API_KEY");
        var url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";
        using var client = new HttpClient();

        _logger.LogInformation($"Calling OpenWeather API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}