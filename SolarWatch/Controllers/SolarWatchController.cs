using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
using SolarWatch.Repository;
using SolarWatch.Service.Processors;
using SolarWatch.Service.Providers;

namespace SolarWatch.Controllers;

public class SolarWatchController : ControllerBase
{
    private readonly ILogger<SolarWatchController> _logger;
    private readonly ILocationDataProcessor _locationDataProcessor;
    private readonly ISunsetSunriseDataProcessor _sunsetSunriseDataProcessor;
    private readonly IWeatherDataProvider _weatherDataProvider;
    private readonly ILocationDataProvider _locationDataProvider;
    private readonly ICityRepository _cityRepository;
    
    public SolarWatchController(ILogger<SolarWatchController> logger
        ,ILocationDataProcessor locationDataProcessor,
        ISunsetSunriseDataProcessor sunriseDataProcessor, IWeatherDataProvider weatherDataProvider, ILocationDataProvider locationDataProvider, ICityRepository cityRepository)
    {
        _locationDataProcessor = locationDataProcessor;
        _sunsetSunriseDataProcessor = sunriseDataProcessor;
        _weatherDataProvider = weatherDataProvider;
        _locationDataProvider = locationDataProvider;
        _cityRepository = cityRepository;
        _logger = logger;
    }
    
    
    [HttpGet]
    [Route("/")]
    public IActionResult Get()
    {

        return Ok(new {message = "Server is running successful."});
    }

    [HttpGet]
    [Route("GetSunsetSunrise")]
    public async Task<ActionResult<CityInformation>> GetSunsetSunrise([Required] string city, [Required] DateTime date)
    {
        try
        {
            var extractedLocationInfo = await _cityRepository.GetLocationDataByCity(city);
            if (extractedLocationInfo == null)
            {
                string rawLocationData = await _locationDataProvider.GetCityLocationData(city);
                extractedLocationInfo = _locationDataProcessor.Process(rawLocationData);
                await _cityRepository.AddLocationData(extractedLocationInfo);
            }

            var extractedCityInformation = await _cityRepository.GetCityByNameAndDate(city, date);
            if (extractedCityInformation == null)
            {
                string rawSunsetSunriseData =
                    await _weatherDataProvider.GetSunsetSunriseData(extractedLocationInfo.Lat, extractedLocationInfo.Lon, date);
                var sunsetSunriseData = _sunsetSunriseDataProcessor.Process(rawSunsetSunriseData);
                extractedCityInformation = new CityInformation
                {
                    City = city,
                    Date = date,
                    Sunrise = sunsetSunriseData.Sunrise,
                    Sunset = sunsetSunriseData.Sunset
                };
                await _cityRepository.AddCityInformation(extractedCityInformation);
            }
            
            return Ok(new CityInformation { City = city, Date = date, Sunrise = extractedCityInformation.Sunrise, Sunset = extractedCityInformation.Sunset});
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "Error getting city data.");
            return NotFound("Error getting city data");
        }
    }
}