using CleanArchitecture.Core.DTOs.User;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.WebApi.Services;
using Microsoft.AspNetCore.Routing;
using MongoDB.Driver;
using MongoDbGenericRepository;
using Moq;
using System.Threading.Tasks;
using MongoDbContext = CleanArchitecture.Infrastructure.Contexts.MongoDbContext;

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

            var mockMongoDatabase = new Mock<IMongoDatabase>(); // Mock MongoDB instance
            var mockDateTimeService = new Mock<IDateTimeService>();
            var mockAuthenticatedUserService = new Mock<IAuthenticatedUserService>();

            var mongoDbContext = new MongoDbContext(mockMongoDatabase.Object, mockDateTimeService.Object, mockAuthenticatedUserService.Object);

            var service = new UpdateUserService(mockUserRepository.Object, mongoDbContext);

            // Act
            var result = await service.UpdateUserProfile(request);

            // Assert
            Assert.Equal(expectedProfile, result);
        }
    }
}
