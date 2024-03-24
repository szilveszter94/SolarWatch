using SolarWatch.Model;

namespace SolarWatch.Service.Processors;

public interface ILocationDataProcessor
{
    LocationData Process(string data);
}