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

            var connectionString = "elifkeskin233:incilemanadventure@adventureallycluster.4ibyufn.mongodb.net/?retryWrites=true&w=majority&appName=AdventureAllyCluster"; // Test veritabaný baðlantý dizesi
            var databaseName = "AdnevtureAllyCluster"; // Test veritabaný adý
            var mongoDbContext = new MongoDbContext(connectionString);

            var service = new UpdateUserService(mockUserRepository.Object, mongoDbContext);
            // Act
            var result = await service.UpdateUserProfile(request);

            // Assert
            Assert.Equal(expectedProfile, result);
        }
    }
}
