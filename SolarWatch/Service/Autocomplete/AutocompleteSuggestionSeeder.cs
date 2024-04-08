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
        var isEmpty = await _cityRepository.GetAllSuggestions();
        if (isEmpty.Count == 0)
        {
            await _cityRepository.PopulateDatabaseWithCitiesFromFile();
        }
    }
}