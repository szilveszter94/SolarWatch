namespace SolarWatch.Service.Providers;

public interface IWeatherDataProvider
{
    Task<string> GetSunsetSunriseData(double lat, double lon, DateTime date);
}