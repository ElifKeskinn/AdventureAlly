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
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis;

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

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);

            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
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
                }), Encoding.UTF8, "application/json")
            };
            mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(httpResponse);

            var service = new GoogleMapsService((HttpClient)mockHttpClientFactory.Object);

            // Act
            var result = await service.GetCoordinatesAsync(address);

            // Assert
            Assert.Equal(expectedCoordinates.Latitude, result.Latitude);
            Assert.Equal(expectedCoordinates.Longitude, result.Longitude);
        }
    }
}
