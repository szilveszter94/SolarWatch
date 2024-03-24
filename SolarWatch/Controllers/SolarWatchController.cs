using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Model;
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
    
    public SolarWatchController(ILogger<SolarWatchController> logger
        ,ILocationDataProcessor locationDataProcessor,
        ISunsetSunriseDataProcessor sunriseDataProcessor, IWeatherDataProvider weatherDataProvider, ILocationDataProvider locationDataProvider)
    {
        _locationDataProcessor = locationDataProcessor;
        _sunsetSunriseDataProcessor = sunriseDataProcessor;
        _weatherDataProvider = weatherDataProvider;
        _locationDataProvider = locationDataProvider;
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
            string rawLocationData = await _locationDataProvider.GetCityLocationData(city);
            var extractedLocationInfo = _locationDataProcessor.Process(rawLocationData);

        
            string rawSunsetSunriseData =
                await _weatherDataProvider.GetSunsetSunriseData(extractedLocationInfo.Lat, extractedLocationInfo.Lon, date);
            var sunsetSunriseData = _sunsetSunriseDataProcessor.Process(rawSunsetSunriseData);
            var extractedCityInformation = new CityInformation
            (
                city,
                date,
                sunsetSunriseData.Sunrise,
                sunsetSunriseData.Sunset
            );
            return Ok(extractedCityInformation);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "Error getting city data.");
            return NotFound("Error getting city data");
        }
    }
}