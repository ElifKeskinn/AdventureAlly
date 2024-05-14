using CleanArchitecture.WebApi.Controllers;
using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Features.Users.Commands.DeleteUserById;
using CleanArchitecture.Core.Features.Users.Commands.UpdateUser;
using CleanArchitecture.Core.Interfaces.Repositories;
using Moq;
using MySqlX.XDevAPI.Common;
using static CleanArchitecture.Core.Features.Users.Commands.DeleteUserById.DeleteUserByIdCommand;
using static CleanArchitecture.Core.Features.Users.Commands.UpdateUser.UpdateUserCommand;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpClient = new Mock<HttpClient>();
            mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedWeatherData)
                });

            var weatherService = new WeatherService(mockHttpClientFactory.Object, "dummy-api-key");
            var weatherController = new WeatherController(weatherService, "dummy-api-key");

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

            var mockWeatherService = new Mock<WeatherService>(MockBehavior.Strict, null, null);
            mockWeatherService.Setup(service => service.GetWeatherAsync(latitude, longitude))
                .ThrowsAsync(new Exception(expectedErrorMessage));

            var weatherController = new WeatherController(mockWeatherService.Object, "dummy-api-key");

            // Act
            var result = await weatherController.GetWeatherForecastAsync(latitude, longitude) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(expectedErrorMessage, result.Value);
        }
    }
}
