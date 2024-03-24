using SolarWatch.Model;

namespace SolarWatch.Service.Processors;

public interface ISunsetSunriseDataProcessor
{
    SunsetSunriseData Process(string data);
}