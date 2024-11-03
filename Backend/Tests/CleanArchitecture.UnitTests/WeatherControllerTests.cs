using CleanArchitecture.WebApi.Configuration;
using CleanArchitecture.WebApi.Controllers;
using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        private readonly IConfiguration _configuration;
       

        public WeatherControllerTests()
        {
            // Build configuration from appsettings.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

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

            var apiSettings = _configuration.GetSection("ApiSettings").Get<ApiSettings>();
            var options = Options.Create(apiSettings);


            var weatherService = new WeatherService(mockHttpClientFactory.Object, options);
            var weatherController = new WeatherController(weatherService, options);



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

            var apiSettings = _configuration.GetSection("ApiSettings").Get<ApiSettings>();
            var options = Options.Create(apiSettings);

            var weatherService = new WeatherService(mockHttpClientFactory.Object, options);
            var weatherController = new WeatherController(weatherService, options);

            // Act
            var result = await weatherController.GetWeatherForecastAsync(latitude, longitude) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains(expectedErrorMessage, result.Value.ToString());
        }
    }
}
