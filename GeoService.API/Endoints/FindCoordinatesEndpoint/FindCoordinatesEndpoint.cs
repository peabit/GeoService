using GeoService.API.CoordinatesSearchService;
using Microsoft.AspNetCore.Mvc;

namespace GeoService.API.Endoints.FindCoordinatesEndpoint;

[ApiController]
[Route("api")]
public sealed class FindCoordinatesEndpoint : ControllerBase
{
    private readonly ICoordinatesSearchService _coordinatesSearchService;

    public FindCoordinatesEndpoint(ICoordinatesSearchService coordinatesSearchService) 
        => _coordinatesSearchService = coordinatesSearchService ?? throw new ArgumentNullException(nameof(coordinatesSearchService));

    [HttpGet("coordinates")]
    public async Task<IActionResult> Handle(string address)
    {
        var coordinates = await _coordinatesSearchService.FindAsync(address);

        if (!coordinates.Any())
        {
            return NotFound();
        }

        return Ok(coordinates);
    }
}