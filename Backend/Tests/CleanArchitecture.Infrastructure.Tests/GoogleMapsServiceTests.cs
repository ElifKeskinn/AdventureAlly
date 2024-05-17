using CleanArchitecture.WebApi.Services;
using Moq;
using Xunit;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models;
using Moq.Protected;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Tests.Services
{
    public class GoogleMapsServiceTests
    {
        [Fact]
        public async Task GetCoordinatesAsync_Returns_Coordinates()
        {
            // Arrange
            string address = "1600 Amphitheatre Parkway, Mountain View, CA"; // Örnek bir adres
            var expectedCoordinates = new Coordinate { Latitude = 37.4224082, Longitude = -122.0856086 }; // Örnek koordinatlar

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new GeocodeResponse
                    {
                        Results = new[]
                        {
                            new GeocodeResult
                            {
                                Geometry = new Geometry
                                {
                                    Location = expectedCoordinates
                                }
                            }
                        }
                    }))
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();
            var service = new GoogleMapsService(httpClient, configuration);

            // Act
            var result = await service.GetCoordinatesAsync(address);

            // Assert
            Assert.Equal(expectedCoordinates.Latitude, result.Latitude);
            Assert.Equal(expectedCoordinates.Longitude, result.Longitude);
        }
    }
}
