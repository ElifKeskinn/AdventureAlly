using CleanArchitecture.Core.DTOs.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Threading.Tasks;

namespace CleanArchitecture.UnitTests.Services
{
    public class UpdateUserServiceTests
    {
        [Fact]
        public async Task UpdateUserProfile_Returns_UpdatedProfile()
        {
            // Arrange
            var request = new UserProfileUpdateRequest(); // �rnek bir istek olu�turun
            var expectedProfile = new UserProfile(); // �rnek bir kullan�c� profili olu�turun

            var mockUserRepository = new Mock<IUserRepositoryAsync>();
            mockUserRepository.Setup(repo => repo.UpdateUserProfile(request)).ReturnsAsync(expectedProfile);

            var service = new UpdateUserService(mockUserRepository.Object);

            // Act
            var result = await service.UpdateUserProfile(request);

            // Assert
            Assert.Equal(expectedProfile, result);
        }
    }
}
