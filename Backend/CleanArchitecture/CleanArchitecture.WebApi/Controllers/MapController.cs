using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.WebApi.Services;
using System;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MapController : ControllerBase
    {
        private readonly GoogleMapsService _googleMapsService;

        public MapController(GoogleMapsService googleMapsService)
        {
            _googleMapsService = googleMapsService;
        }

        [HttpGet("coordinates")]
        public async Task<IActionResult> GetCoordinates([FromQuery] string address)
        {
            try
            {
                var coordinates = await _googleMapsService.GetCoordinatesAsync(address);
                var response = new
                {
                    Latitude = coordinates.Latitude,
                    Longitude = coordinates.Longitude
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
