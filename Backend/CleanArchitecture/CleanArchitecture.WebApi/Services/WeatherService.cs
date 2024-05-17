using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Services
{
    public class WeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;

        public WeatherService(IHttpClientFactory httpClientFactory, string apiKey)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = apiKey;
        }

        public async Task<string> GetWeatherAsync(double latitude, double longitude)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_apiKey}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    throw new Exception("Failed to get weather data.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving weather data: {ex.Message}");
            }
        }
    }
}
