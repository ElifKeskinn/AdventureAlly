using CleanArchitecture.WebApi.Controllers;
using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Controllers
{
    public class WeatherControllerTests
    {
        [Fact]
        public async Task GetWeatherForecastAsync_Returns_OkObjectResult_With_WeatherData()
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

            var weatherService = new WeatherService(mockHttpClientFactory.Object, "38f9fbea711c5c8c97baceec7c5b356c");
            var weatherController = new WeatherController(weatherService, "38f9fbea711c5c8c97baceec7c5b356c");

            // Act
            var result = await weatherController.GetWeatherForecastAsync(latitude, longitude) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedWeatherData, result.Value);
        }

        [Fact]
        public async Task GetWeatherForecastAsync_Returns_InternalServerError_When_ServiceThrowsException()
        {
            // Arrange
            double latitude = 40.7128; // New York
            double longitude = -74.0060;
            var expectedErrorMessage = "An error occurred while retrieving weather data.";

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception(expectedErrorMessage));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var weatherService = new WeatherService(mockHttpClientFactory.Object, "38f9fbea711c5c8c97baceec7c5b356c");
            var weatherController = new WeatherController(weatherService, "38f9fbea711c5c8c97baceec7c5b356c");

            // Act
            var result = await weatherController.GetWeatherForecastAsync(latitude, longitude) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(expectedErrorMessage, result.Value.ToString());
        }
    }
}
