using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models;

namespace CleanArchitecture.WebApi.Services
{
    public class GoogleMapsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GoogleMapsService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = "AIzaSyC3HtB9t90I8RiOmNUJhNAZ2D__-RM-X-U"; 
        }

        public async Task<Coordinate> GetCoordinatesAsync(string address)
        {
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";

            using (var response = await _httpClient.GetAsync(url))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to retrieve coordinates.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GeocodeResponse>(content);

                if (result == null || result.Results.Length == 0)
                {
                    throw new Exception("No coordinates found for the given address.");
                }

                var coordinates = new Coordinate
                {
                    Latitude = result.Results[0].Geometry.Location.Latitude,
                    Longitude = result.Results[0].Geometry.Location.Longitude
                };

                return coordinates;
            }
        }
    }
}
