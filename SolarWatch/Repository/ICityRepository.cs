using SolarWatch.Model;
using SolarWatch.Model.CreateModels;

namespace SolarWatch.Repository;

public interface ICityRepository
{
    Task<CityInformation?> GetCityByNameAndDate(string name, DateTime date);
    Task<List<CityInformation>?> GetAllCityInformation();
    Task<CityInformation?> GetCityInformationById(int id);
    Task<LocationData?> GetLocationDataByCity(string city);
    Task<List<LocationData>?> GetAllLocationData();
    Task<LocationData?> GetLocationDataById(int id);
    Task<CityInformation> AddCityInformation(CityInfoRequest cityInformation);
    Task<CityInformation> AddCityInformation(CityInformation cityInformation);
    Task<LocationData> AddLocationData(LocationDataRequest locationData);
    Task<LocationData> AddLocationData(LocationData locationData);
    Task<LocationData> UpdateLocationData(LocationData locationData);
    Task<CityInformation> UpdateCityInformation(CityInformation cityInformation);
    Task DeleteCityInformation(int id);
    Task DeleteLocationData(int id);
}