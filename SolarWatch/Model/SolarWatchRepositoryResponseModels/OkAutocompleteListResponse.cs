namespace SolarWatch.Model.SolarWatchRepositoryResponseModels;

public record OkAutocompleteListResponse(string Message, List<AutocompleteCityModel> Data);