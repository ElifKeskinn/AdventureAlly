using CleanArchitecture.WebApi.Configuration;
using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using CleanArchitecture.WebApi.Configuration; // Add this namespace
using Microsoft.Extensions.Options; // Add this namespace

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly ApiSettings _apiSettings;
        public WeatherController(WeatherService weatherService, IOptions<ApiSettings> apiSettings)
        {
            _weatherService = weatherService;
            _apiSettings = apiSettings.Value;
        }

        [HttpGet("forecast")]
        public async Task<IActionResult> GetWeatherForecastAsync(double latitude, double longitude)
        {
            try
            {
                var weatherData = await _weatherService.GetWeatherAsync(latitude, longitude);
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving weather data: {ex.Message}");
            }
        }
    }}