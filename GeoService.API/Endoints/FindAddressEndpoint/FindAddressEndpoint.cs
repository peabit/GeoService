using GeoService.API.Domain.AdressesSearchService;
using Microsoft.AspNetCore.Mvc;

namespace GeoService.API.Endoints.FindCoordinatesEndpoint;

[ApiController]
[Route("api")]
public sealed class FindAddressEndpoint : ControllerBase
{
    private readonly IAdressesSearchService _coordinatesSearchService;

    public FindAddressEndpoint(IAdressesSearchService coordinatesSearchService) 
        => _coordinatesSearchService = coordinatesSearchService ?? throw new ArgumentNullException(nameof(coordinatesSearchService));

    [HttpGet("addresses")]
    public async Task<IActionResult> Handle(string searchTerm)
    {
        var coordinates = await _coordinatesSearchService.FindAsync(searchTerm);

        if (coordinates.Any())
        {
            return Ok(coordinates);  
        }

        return NotFound();
    }
}