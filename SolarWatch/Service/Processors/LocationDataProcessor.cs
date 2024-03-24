using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Service.Processors;

public class LocationDataProcessor : ILocationDataProcessor
{
    public LocationData Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        string city = json.RootElement[0].GetProperty("name").GetString() ?? "";
        double lat = json.RootElement[0].GetProperty("lat").GetDouble();
        double lon = json.RootElement[0].GetProperty("lon").GetDouble();

        return new LocationData { City = city, Lat = lat, Lon = lon };
    }
}