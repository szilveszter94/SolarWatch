namespace SolarWatch.Service.Providers;

public interface ILocationDataProvider
{
    Task<string> GetCityLocationData(string city);
}