namespace GeoService.API.Infrastructure.NearbyAddressSearchService;

public sealed record DadataConfig(
    string Token,
    int SearchRadiusMeters
);