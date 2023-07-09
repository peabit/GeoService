using GeoService.API.CoordinatesProvider;
using GeoService.API.CoordinatesSearchService;
using Microsoft.Net.Http.Headers;

namespace GeoService.API.Infrastructure.CoordinatesSearchService;

public sealed class OsmCoordinatesSearchService : ICoordinatesSearchService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OsmCoordinatesSearchService(IHttpClientFactory httpClientFactory)
        => _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

    public async Task<IEnumerable<CoordinatesDto>> FindAsync(string address)
    {
        EnsureValidAddress(address);

        using var httpClient = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://nominatim.openstreetmap.org/search?q={address}&format=json")
        {
            Headers = { { HeaderNames.UserAgent, "Other" } }
        };

        using var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var rawCoordiantes = await response.Content.ReadFromJsonAsync<IEnumerable<Model>>();

        var coordinates = rawCoordiantes!.Select(c => new CoordinatesDto(
            Address: c.display_name,
            Longitude: c.lon,
            Latitude: c.lat
        ));

        return coordinates;
    }

    private void EnsureValidAddress(string address)
    {
        if (address is null)
        {
            throw new ArgumentNullException(nameof(address));
        }

        if (String.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address ", nameof(address));
        }
    }

    private sealed record Model(string display_name, string lat, string lon);
}