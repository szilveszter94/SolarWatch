using System.Runtime.InteropServices;
using SolarWatch.Model.CreateModels;
using SolarWatch.Model.SolarWatchRepositoryResponseModels;

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
        return Ok(new MessageResponse("Server is running."));
    }
    
    [HttpGet, Authorize(Roles = "Admin, User")]
    [Route("GetCitiesForAutocomplete")]
    public async Task<ActionResult<List<AutocompleteCityModel>>> GetCitiesForAutocomplete([Required] string suggestion)
    {
        try
        {
            var result = await _cityRepository.GetCityBySuggestion(suggestion);
            return Ok(new OkAutocompleteListResponse("Cities retrieved successfully", result));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new MessageResponse("Cannot get cities. An error occured."));
        }
    }

    [HttpGet, Authorize(Roles = "Admin, User")]
    [Route("GetSunsetSunrise")]
    public async Task<ActionResult<OkCityInformationListResponse>> GetSunsetSunrise([Required] string city, [Required] DateTime date, int forecast = 1)
    {
        try
        {
            var results = new List<CityInformation>();
            for (int i = 0; i < forecast; i++)
            {
                var newDate = date.AddDays(i);
                var result = await FetchCityInformation(city, newDate);
                results.Add(result);
            }
            return Ok(new OkCityInformationListResponse("Successfully retrieved data", results.ToList())); 
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, "Error getting city data.");
            return NotFound(new MessageResponse("Error getting city data"));
        }
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("GetAllCityInformation")]
    public async Task<ActionResult<CityInformation>> GetAllCityInformation()
    {
        try
        {
            var result = await _cityRepository.GetAllCityInformation();
            return Ok(new OkCityInformationListResponse ("Information retrieved successfully", result));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new MessageResponse("Cannot get information. An error occured."));
        }
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("GetCityInformationById/{id}")]
    public async Task<ActionResult<CityInformation>> GetCityInformationById(int id)
    {
        try
        {
            var result = await _cityRepository.GetCityInformationById(id);
            return Ok(new OkCityInformationResponse("Information retrieved successfully", result));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(new MessageResponse("Cannot get information. An error occured."));
        }
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("GetAllLocationData")]
    public async Task<ActionResult<LocationData>> GetAllLocationData()
    {
        try
        {
            var result = await _cityRepository.GetAllLocationData();
            return Ok(new OkLocationDataListResponse("Locations retrieved successfully", result));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new MessageResponse("Cannot get locations. An error occured."));
        }
    }
    
    [HttpGet, Authorize(Roles = "Admin")]
    [Route("GetLocationDataById/{id}")]
    public async Task<ActionResult<LocationData>> GetLocationDataById(int id)
    {
        try
        {
            var result = await _cityRepository.GetLocationDataById(id);
            return Ok(new OkLocationDataResponse( "Information retrieved successfully", result));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound(new MessageResponse("Cannot get information. An error occured."));
        }
    }
    
    [HttpPost, Authorize(Roles = "Admin")]
    [Route("AddCityInformation")]
    public async Task<ActionResult<CityInformation>> AddCityInformation([Required] CityInfoRequest cityInformation)
    {
        try
        {
            var result = await _cityRepository.AddCityInformation(cityInformation);
            return Ok(new OkCityInformationResponse("Information created successfully", result));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new MessageResponse(e.Message));
        }
    }
    
    [HttpPost, Authorize(Roles = "Admin")]
    [Route("AddLocationData")]
    public async Task<ActionResult<LocationData>> AddLocationData([Required] LocationDataRequest locationData)
    {
        try
        {
            var result = await _cityRepository.AddLocationData(locationData);
            return Ok(new OkLocationDataResponse( "Location created successfully", result ));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new MessageResponse(e.Message));
        }
    }
    
    [HttpPatch, Authorize(Roles = "Admin")]
    [Route("UpdateCityInformation")]
    public async Task<ActionResult<CityInformation>> UpdateCityInformation([Required] CityInformation cityInformation)
    {
        try
        {
            var updatedCity = await _cityRepository.UpdateCityInformation(cityInformation);
            return Ok(new OkCityInformationResponse("City updated successfully", updatedCity));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new MessageResponse("Update failed"));
        }
    }
    
    [HttpPatch, Authorize(Roles = "Admin")]
    [Route("UpdateLocationData")]
    public async Task<ActionResult<CityInformation>> UpdateLocationData([Required] LocationData locationData)
    {
        try
        {
            var updatedLocation = await _cityRepository.UpdateLocationData(locationData);
            return Ok(new OkLocationDataResponse("Location updated successfully", updatedLocation));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new MessageResponse("Update failed"));
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
            return BadRequest(new MessageResponse("Delete failed."));
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
            return BadRequest(new MessageResponse("Delete failed."));
        }
    }
    
    private async Task<CityInformation> FetchCityInformation(string city, DateTime date)
    {
        var extractedLocationInfo = await _cityRepository.GetLocationDataByCity(city);
        if (extractedLocationInfo == null)
        {
            string rawLocationData = await _locationDataProvider.GetCityLocationData(city);
            extractedLocationInfo = _locationDataProcessor.Process(rawLocationData);
            await _cityRepository.AddLocationData(extractedLocationInfo);
        }

        var extractedCityInformation = await _cityRepository.GetCityByNameAndDate(extractedLocationInfo.City, date);
        if (extractedCityInformation == null)
        {
            string rawSunsetSunriseData = await _weatherDataProvider.GetSunsetSunriseData(extractedLocationInfo.Lat, extractedLocationInfo.Lon, date);
            var sunsetSunriseData = _sunsetSunriseDataProcessor.Process(rawSunsetSunriseData);
            extractedCityInformation = new CityInformation
            {
                City = extractedLocationInfo.City,
                Date = date,
                Sunrise = sunsetSunriseData.Sunrise,
                Sunset = sunsetSunriseData.Sunset
            };
            await _cityRepository.AddCityInformation(extractedCityInformation);
        }
        await _cityRepository.AddAutocompleteSuggestion(extractedCityInformation.City);
        return extractedCityInformation;
    }
}