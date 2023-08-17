using GeoService.API.Domain.AdressesSearchService;
using Microsoft.Net.Http.Headers;

namespace GeoService.API.Infrastructure.CoordinatesSearchService;

public sealed class OsmAdressesSearchService : IAdressesSearchService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OsmAdressesSearchService(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

    public async Task<IEnumerable<AddressDto>> FindAsync(string searchTerm)
    {
        EnsureValidSearchTerm(searchTerm);

        using var httpClient = _httpClientFactory.CreateClient();

        var request = CreateRequest(searchTerm);

        using var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var rawCoordiantes = await response.Content.ReadFromJsonAsync<IEnumerable<Model>>();

        var coordinates = MapResponse(rawCoordiantes!);

        return coordinates;
    }

    private static void EnsureValidSearchTerm(string address)
    {
        if (address is null)
        {
            throw new ArgumentNullException(nameof(address));
        }

        if (String.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address cannot be empty", nameof(address));
        }
    }

    private static HttpRequestMessage CreateRequest(string address)
        => new(HttpMethod.Get, $"https://nominatim.openstreetmap.org/search?q={address}&format=json")
        {
            Headers = { { HeaderNames.UserAgent, "Other" } }
        };

    private static IEnumerable<AddressDto> MapResponse(IEnumerable<Model> rawCoordiantes)
        => rawCoordiantes!.Select(
            c => new AddressDto(
                Address: c.display_name,
                Lat: c.lat,
                Lon: c.lon
        ));

    private sealed record Model(string display_name, string lon, string lat);
}