namespace GeoService.API.Domain.NearbyAddressSearchService;

public interface INearbyAddressSearchService
{
    Task<IEnumerable<NearbyAddressDto>> Find(double lat, double lon);
}