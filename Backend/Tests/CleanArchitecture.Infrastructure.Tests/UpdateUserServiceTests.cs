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
            var request = new UserProfileUpdateRequest(); // Örnek bir istek oluþturun
            var expectedProfile = new UserProfile(); // Örnek bir kullanýcý profili oluþturun

            var mockUserRepository = new Mock<IUserRepositoryAsync>();
            mockUserRepository.Setup(repo => repo.UpdateUserProfile(request)).ReturnsAsync(expectedProfile);

            var connectionString = "mongodb://elif:elifkeskin@localhost:27017/"; // Test veritabaný baðlantý dizesi
            var databaseName = "adventureAlly"; // Test veritabaný adý
            var mongoDbContext = new MongoDbContext(connectionString);

            var service = new UpdateUserService(mockUserRepository.Object, mongoDbContext);
            // Act
            var result = await service.UpdateUserProfile(request);

            // Assert
            Assert.Equal(expectedProfile, result);
        }
    }
}
