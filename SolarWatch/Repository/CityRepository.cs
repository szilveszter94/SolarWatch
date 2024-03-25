using SolarWatch.Model.CreateModels;

namespace SolarWatch.Repository;

using Microsoft.EntityFrameworkCore;
using Context;
using Model;

public class CityRepository : ICityRepository
{
    private readonly SolarWatchContext _cityContext;
    
    public CityRepository(SolarWatchContext cityContext)
    {
        _cityContext = cityContext;
    }
    public async Task<CityInformation?> GetCityByNameAndDate(string name, DateTime date)
    {
        return await _cityContext.CityInformations.FirstOrDefaultAsync(c => c.City == name && c.Date == date);
    }
    
    public async Task<LocationData?> GetLocationDataByCity(string city)
    {
        return await _cityContext.LocationDatas.FirstOrDefaultAsync(c => c.City == city);
    }
    
    public async Task<CityInformation> AddCityInformation(CityInfoRequest cityInformation)
    {
        var cityToCreate = new CityInformation()
        {
            City = cityInformation.City, Date = cityInformation.Date, Sunrise = cityInformation.Sunrise,
            Sunset = cityInformation.Sunset
        };
        var newCity = await _cityContext.CityInformations.AddAsync(cityToCreate);
        await _cityContext.SaveChangesAsync();
        return newCity.Entity;
    }
    
    public async Task<CityInformation> AddCityInformation(CityInformation cityInformation)
    {
        var newCity = await _cityContext.CityInformations.AddAsync(cityInformation);
        await _cityContext.SaveChangesAsync();
        return newCity.Entity;
    }
    
    public async Task<LocationData> AddLocationData(LocationDataRequest locationData)
    {
        var locationToCreate = new LocationData()
        {
            City = locationData.City, Lat = locationData.Lat, Lon = locationData.Lon
        };
        var newLocation = await _cityContext.LocationDatas.AddAsync(locationToCreate);
        await _cityContext.SaveChangesAsync();
        return newLocation.Entity;
    }
    
    public async Task<LocationData> AddLocationData(LocationData locationData)
    {
        var newLocation = await _cityContext.LocationDatas.AddAsync(locationData);
        await _cityContext.SaveChangesAsync();
        return newLocation.Entity;
    }
    
    public async Task<LocationData> UpdateLocationData(LocationData locationData)
    {
        var existingLocation = await _cityContext.LocationDatas.FirstOrDefaultAsync(c => c.Id == locationData.Id);

        if (existingLocation == null)
        {
            throw new KeyNotFoundException("Failed to update. Location not found.");
        }
            
        _cityContext.LocationDatas.Entry(existingLocation).CurrentValues.SetValues(locationData);
        await _cityContext.SaveChangesAsync();

        return existingLocation;
    }
    
    public async Task<CityInformation> UpdateCityInformation(CityInformation cityInformation)
    {
        var existingCity = await _cityContext.CityInformations.FirstOrDefaultAsync(c => c.Id == cityInformation.Id);

        if (existingCity == null)
        {
            throw new KeyNotFoundException("Failed to update. Location not found.");
        }
            
        _cityContext.CityInformations.Entry(existingCity).CurrentValues.SetValues(cityInformation);
        await _cityContext.SaveChangesAsync();

        return existingCity;
    }
    
    public async Task DeleteCityInformation(int id)
    {
        try
        {
            var cityInformationToDelete = await _cityContext.CityInformations.FirstOrDefaultAsync(c => c.Id == id);

            if (cityInformationToDelete == null)
            {
                throw new KeyNotFoundException("Failed to delete. City information not found.");
            }
            
            _cityContext.CityInformations.Remove(cityInformationToDelete);
            await _cityContext.SaveChangesAsync();
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("City information not deleted. An unexpected error occured.");
        }
    }
    
    public async Task DeleteLocationData(int id)
    {
        try
        {
            var locationDataToDelete = await _cityContext.LocationDatas.FirstOrDefaultAsync(c => c.Id == id);

            if (locationDataToDelete == null)
            {
                throw new KeyNotFoundException("Failed to delete. Location not found.");
            }
            
            _cityContext.LocationDatas.Remove(locationDataToDelete);
            await _cityContext.SaveChangesAsync();
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Location not deleted. An unexpected error occured.");
        }
    }
}