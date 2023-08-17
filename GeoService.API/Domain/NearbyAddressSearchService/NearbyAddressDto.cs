namespace GeoService.API.Domain.NearbyAddressSearchService;

public sealed record NearbyAddressDto(
    string City,
    string Street,
    string House,
    string Lon,
    string Lat
);