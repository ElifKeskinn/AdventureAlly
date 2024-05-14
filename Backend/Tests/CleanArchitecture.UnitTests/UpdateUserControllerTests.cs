using CleanArchitecture.Core.DTOs.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;

namespace CleanArchitecture.UnitTests.Controllers
{
    public class UpdateUserControllerTests
    {
        [Fact]
        public async Task UpdateUserProfile_Returns_OkResult_With_UpdatedProfile()
        {
            // Arrange
            var request = new UserProfileUpdateRequest(); // �rnek bir istek olu�turun
            var expectedProfile = new UserProfile(); // �rnek bir kullan�c� profili olu�turun

            var mockUpdateUserService = new Mock<IUpdateUserService>();
            mockUpdateUserService.Setup(service => service.UpdateUserProfile(request)).ReturnsAsync(expectedProfile);

            var controller = new UpdateUserController(mockUpdateUserService.Object);

            // Act
            var result = await controller.UpdateUserProfile(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal((int)System.Net.HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedProfile, result.Value);
        }
    }
}
