using GeoService.API.CoordinatesProvider;

namespace GeoService.API.CoordinatesSearchService;

public interface ICoordinatesSearchService
{
    Task<IEnumerable<CoordinatesDto>> FindAsync(string address);
}