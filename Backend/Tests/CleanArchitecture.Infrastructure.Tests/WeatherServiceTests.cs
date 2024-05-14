using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Services
{
  
    public class WeatherServiceTest
    {
        [Fact]
        public async Task GetWeatherAsync_Returns_WeatherData()
        {
            // Arrange
            double latitude = 40.7128; // New York
            double longitude = -74.0060;
            var expectedWeatherData = "{ 'temperature': 25, 'condition': 'Sunny' }"; // Example weather data

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedWeatherData)
                });

            var service = new WeatherService(mockHttpClientFactory.Object, "38f9fbea711c5c8c97baceec7c5b356c");

            // Act
            var result = await service.GetWeatherAsync(latitude, longitude);

            // Assert
            Assert.Equal(expectedWeatherData, result);
        }

        [Fact]
        public void GetWeatherAsync_Throws_Exception_On_Failure()
        {
            // Arrange
            double latitude = 40.7128; // New York
            double longitude = -74.0060;

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
            mockHttpClient.SetupSequence(client => client.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ThrowsAsync(new Exception("Failed to get weather data."))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) // Durum kodu burada belirtilmeli
                {
                    Content = new StringContent("{ 'temperature': 25, 'condition': 'Sunny' }")
                });


            var service = new WeatherService(mockHttpClientFactory.Object, "38f9fbea711c5c8c97baceec7c5b356c");

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await service.GetWeatherAsync(latitude, longitude));
        }
    }
}
