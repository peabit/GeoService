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
        throw new Exception("ЫВАыВаываыва");

        EnsureValidAddress(address);

        using var httpClient = _httpClientFactory.CreateClient();

        var request = CreateRequest(address);

        using var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var rawCoordiantes = await response.Content.ReadFromJsonAsync<IEnumerable<Model>>();

        var coordinates = Map(rawCoordiantes!);

        return coordinates;
    }

    private static void EnsureValidAddress(string address)
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

    private static IEnumerable<CoordinatesDto> Map(IEnumerable<Model> rawCoordiantes)
        => rawCoordiantes!.Select(c => new CoordinatesDto(
            Address: c.display_name,
            Longitude: c.lon,
            Latitude: c.lat
        ));

    private sealed record Model(string display_name, string lon, string lat);
}