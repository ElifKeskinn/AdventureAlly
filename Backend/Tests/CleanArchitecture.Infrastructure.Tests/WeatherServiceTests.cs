using CleanArchitecture.WebApi.Configuration;
using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var apiKey = configuration.GetValue<string>("ApiSettings:ApiKey");

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
            var mockOptions = new Mock<IOptions<ApiSettings>>();
            mockOptions.Setup(x => x.Value).Returns(new ApiSettings { ApiKey = apiKey });
            var service = new WeatherService(mockHttpClientFactory.Object, mockOptions.Object);

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
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var weatherApiKey = configuration.GetValue<string>("ApiSettings:ApiKey");
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockOptions = new Mock<IOptions<ApiSettings>>();
            mockOptions.Setup(x => x.Value).Returns(new ApiSettings { ApiKey = weatherApiKey });

            var service = new WeatherService(mockHttpClientFactory.Object, mockOptions.Object);            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await service.GetWeatherAsync(latitude, longitude));
        }
    }
}
