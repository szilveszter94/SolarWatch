using Microsoft.EntityFrameworkCore;
using SolarWatch.Context;
using SolarWatch.Model;

namespace SolarWatch.Repository;

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
    
    public async Task AddCityInformation(CityInformation cityInformation)
    {
        await _cityContext.AddAsync(cityInformation);
        await _cityContext.SaveChangesAsync();
    }
    
    public async Task AddLocationData(LocationData locationData)
    {
        await _cityContext.AddAsync(locationData);
        await _cityContext.SaveChangesAsync();
    }
    
    public async Task<LocationData> UpdateLocationData(LocationData locationData)
    {
        var existingLocation = await _cityContext.LocationDatas.FirstOrDefaultAsync(c => c.Id == locationData.Id);

        if (existingLocation == null)
        {
            throw new KeyNotFoundException("Failed to update. Location not found.");
        }
            
        _cityContext.Entry(existingLocation).CurrentValues.SetValues(locationData);
        await _cityContext.SaveChangesAsync();

        return existingLocation;
    }
}