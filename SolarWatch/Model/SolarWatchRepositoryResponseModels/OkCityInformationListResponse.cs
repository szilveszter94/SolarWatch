namespace SolarWatch.Model.SolarWatchRepositoryResponseModels;

public record OkCityInformationListResponse(string Message, List<CityInformation>? Data);