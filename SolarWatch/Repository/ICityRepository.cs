using SolarWatch.Model;

namespace SolarWatch.Repository;

public interface ICityRepository
{
    Task<CityInformation?> GetCityByNameAndDate(string name, DateTime date);
    Task<LocationData?> GetLocationDataByCity(string city);
    Task AddCityInformation(CityInformation cityInformation);
    Task AddLocationData(LocationData locationData);
    Task<LocationData> UpdateLocationData(LocationData locationData);
}