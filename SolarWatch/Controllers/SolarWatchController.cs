using SolarWatch.Model.CreateModels;

namespace SolarWatch.Controllers;

using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Model;
using Repository;
using Service.Processors;
using Service.Providers;

[ApiController]
[Route("[controller]")]
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
        return Ok(new {message = "Server is running."});
    }

    [HttpGet, Authorize(Roles = "Admin, User")]
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
            
            return Ok(new {message = "Successfully retrieved data", data = new CityInformation { City = city, Date = date, Sunrise = extractedCityInformation.Sunrise, Sunset = extractedCityInformation.Sunset}});
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "Error getting city data.");
            return NotFound(new {message = "Error getting city data"});
        }
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("GetAllCityInformation")]
    public async Task<ActionResult<CityInformation>> GetAllCityInformation()
    {
        try
        {
            var result = await _cityRepository.GetAllCityInformation();
            return Ok(new {message = "Information retrieved successfully", data=result});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {message = "Cannot get information. An error occured."});
        }
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("GetCityInformationById/{id}")]
    public async Task<ActionResult<CityInformation>> GetCityInformationById(int id)
    {
        try
        {
            var result = await _cityRepository.GetCityInformationById(id);
            return Ok(new {message = "Information retrieved successfully", data=result});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {message = "Cannot get information. An error occured."});
        }
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("GetAllLocationData")]
    public async Task<ActionResult<LocationData>> GetAllLocationData()
    {
        try
        {
            var result = await _cityRepository.GetAllLocationData();
            return Ok(new {message = "Locations retrieved successfully", data=result});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {message = "Cannot get locations. An error occured."});
        }
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("GetLocationDataById/{id}")]
    public async Task<ActionResult<LocationData>> GetLocationDataById(int id)
    {
        try
        {
            var result = await _cityRepository.GetLocationDataById(id);
            return Ok(new {message = "Information retrieved successfully", data=result});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {message = "Cannot get information. An error occured."});
        }
    }
    
    [HttpPost, Authorize(Roles = "Admin")]
    [Route("AddCityInformation")]
    public async Task<ActionResult<CityInformation>> AddCityInformation([Required] CityInfoRequest cityInformation)
    {
        try
        {
            var result = await _cityRepository.AddCityInformation(cityInformation);
            return Ok(new {message = "Information created successfully", data=result});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {message = e.Message});
        }
    }
    
    [HttpPost, Authorize(Roles = "Admin")]
    [Route("AddLocationData")]
    public async Task<ActionResult<LocationData>> AddLocationData([Required] LocationDataRequest locationData)
    {
        try
        {
            var result = await _cityRepository.AddLocationData(locationData);
            return Ok(new {message = "Location created successfully", data=result});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {message = e.Message});
        }
    }
    
    [HttpPatch, Authorize(Roles = "Admin")]
    [Route("UpdateCityInformation")]
    public async Task<ActionResult<CityInformation>> UpdateCityInformation([Required] CityInformation cityInformation)
    {
        try
        {
            var updatedCity = await _cityRepository.UpdateCityInformation(cityInformation);
            return Ok(updatedCity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Update failed");
        }
    }
    
    [HttpPatch, Authorize(Roles = "Admin")]
    [Route("UpdateLocationData")]
    public async Task<ActionResult<CityInformation>> UpdateLocationData([Required] LocationData locationData)
    {
        try
        {
            var updatedLocation = await _cityRepository.UpdateLocationData(locationData);
            return Ok(updatedLocation);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Update failed");
        }
    }
    
    [HttpDelete, Authorize(Roles = "Admin")]
    [Route("DeleteCityInformation")]
    public async Task<ActionResult<CityInformation>> DeleteCityInformation([Required] int id)
    {
        try
        {
            await _cityRepository.DeleteCityInformation(id);
            return Ok(new {message = "Delete successful."});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {message = "Delete failed."});
        }
    }
    
    [HttpDelete, Authorize(Roles = "Admin")]
    [Route("DeleteLocationData")]
    public async Task<ActionResult<CityInformation>> DeleteLocationData([Required] int id)
    {
        try
        {
            await _cityRepository.DeleteLocationData(id);
            return Ok(new {message = "Delete successful."});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {message = "Delete failed."});
        }
    }
}