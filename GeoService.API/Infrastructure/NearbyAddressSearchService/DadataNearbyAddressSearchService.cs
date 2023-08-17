using Dadata;
using Dadata.Model;
using GeoService.API.Domain.NearbyAddressSearchService;

namespace GeoService.API.Infrastructure.NearbyAddressSearchService;

public class DadataNearbyAddressSearchService : INearbyAddressSearchService
{
    private readonly SuggestClientAsync _api;
    private readonly int _searchRadiusMeters;

    public DadataNearbyAddressSearchService(DadataConfig config)
    {
        _searchRadiusMeters = config.SearchRadiusMeters;
        _api = new SuggestClientAsync(token: config.Token);
    }

    public async Task<IEnumerable<NearbyAddressDto>> Find(double lat, double lon)
    {
        var response = await _api.Geolocate(lat, lon, count: 10, radius_meters: _searchRadiusMeters);
        return MapResponse(response);
    }

    private static IEnumerable<NearbyAddressDto> MapResponse(SuggestResponse<Address> response)
        => response.suggestions.Select(
            s => new NearbyAddressDto(
                City: s.data.city,
                Street: s.data.street,
                House: s.data.house,
                Lat: s.data.geo_lat,
                Lon: s.data.geo_lon)
        );
}