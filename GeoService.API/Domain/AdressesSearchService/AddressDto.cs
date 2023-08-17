namespace GeoService.API.Domain.AdressesSearchService;

public sealed record AddressDto(
    string Address, 
    string Lat, 
    string Lon
);