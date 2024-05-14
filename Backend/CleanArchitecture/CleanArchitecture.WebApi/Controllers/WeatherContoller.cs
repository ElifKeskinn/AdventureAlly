using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly string _apiKey;
        public WeatherController(WeatherService weatherService, string apiKey)
        {
            _weatherService = weatherService;
            _apiKey = apiKey;
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