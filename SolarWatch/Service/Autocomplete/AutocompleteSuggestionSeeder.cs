using SolarWatch.Repository;

namespace SolarWatch.Service.Autocomplete;

public class AutocompleteSuggestionSeeder : IAutocompleteSuggestionSeeder
{
    private readonly ICityRepository _cityRepository;
    
    public AutocompleteSuggestionSeeder(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public void PupulateAutocompleteTable()
    {
        var tPopulate = PopulateAutocompleteTableIfEmpty();
        tPopulate.Wait();
    }

    private async Task PopulateAutocompleteTableIfEmpty()
    {
        var isAny = await _cityRepository.IsAnySuggestion();
        if (!isAny)
        {
            await _cityRepository.PopulateDatabaseWithCitiesFromFile();
        }
    }
}