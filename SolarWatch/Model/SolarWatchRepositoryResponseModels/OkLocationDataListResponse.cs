namespace SolarWatch.Model.SolarWatchRepositoryResponseModels;

public record OkLocationDataListResponse(string Message, List<LocationData>? Data);