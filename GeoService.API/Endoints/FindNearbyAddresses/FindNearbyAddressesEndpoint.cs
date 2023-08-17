using GeoService.API.Domain.NearbyAddressSearchService;
using Microsoft.AspNetCore.Mvc;

namespace GeoService.API.Endoints.FindNearbyAddresses;

[ApiController]
[Route("api")]
public class FindNearbyAddresses : ControllerBase
{
    private readonly INearbyAddressSearchService _nearbyAddressSearchService;

    public FindNearbyAddresses(INearbyAddressSearchService nearbyAddressSearchService)
        => _nearbyAddressSearchService = nearbyAddressSearchService ?? throw new ArgumentNullException(nameof(nearbyAddressSearchService));

    [HttpGet("nearby-addresses{lat:double}&{lon:double}")]
    public async Task<IActionResult> Handle(double lat, double lon)
    {
        var addresses = await _nearbyAddressSearchService.Find(lat, lon);

        if (addresses.Any())
        {
            return Ok(addresses);
        }

        return NotFound();
    }
}