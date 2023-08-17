namespace GeoService.API.Domain.AdressesSearchService;

public interface IAdressesSearchService
{
    Task<IEnumerable<AddressDto>> FindAsync(string searchTerm);
}