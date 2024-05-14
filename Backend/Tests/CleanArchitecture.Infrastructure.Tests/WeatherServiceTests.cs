using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Routing;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedWeatherData)
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new WeatherService(mockHttpClientFactory.Object, "38f9fbea711c5c8c97baceec7c5b356c");

            // Act
            var result = await service.GetWeatherAsync(latitude, longitude);

            // Assert
            Assert.Equal(expectedWeatherData, result);
        }

        [Fact]
        public async Task GetWeatherAsync_Throws_Exception_On_Failure()
        {
            // Arrange
            double latitude = 40.7128; // New York
            double longitude = -74.0060;

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var service = new WeatherService(mockHttpClientFactory.Object, "38f9fbea711c5c8c97baceec7c5b356c");

            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.GetWeatherAsync(latitude, longitude));
        }
    }
}
