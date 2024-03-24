using System.Text.Json;
using SolarWatch.Model;

namespace SolarWatch.Service.Processors;

public class SunsetSunriseDataProcessor : ISunsetSunriseDataProcessor
{
    public SunsetSunriseData Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");
        string sunrise = results.GetProperty("sunrise").GetString() ?? "";
        string sunset = results.GetProperty("sunset").GetString() ?? "";

        return new SunsetSunriseData(sunrise, sunset);
    }
}