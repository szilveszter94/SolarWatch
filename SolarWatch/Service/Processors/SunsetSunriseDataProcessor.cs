namespace SolarWatch.Service.Processors;

using System.Text.Json;
using Model;

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