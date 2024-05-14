using AutoFixture;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CleanArchitecture.Infrastructure.Tests
{
    public class UserRepositoryTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IDateTimeService> _dateTimeService;
        private readonly Mock<IAuthenticatedUserService> _authenticatedUserService;
        private readonly User existingUser;
        private readonly ApplicationDbContext context;


        public UserRepositoryTest() {

            this._fixture = new Fixture();
            this.existingUser = _fixture.Create<User>();
            _dateTimeService = new Mock<IDateTimeService>();
            _authenticatedUserService = new Mock<IAuthenticatedUserService>();  

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(_fixture.Create<string>());

            context = new ApplicationDbContext(optionsBuilder.Options, _dateTimeService.Object, _authenticatedUserService.Object);

            context.Users.Add(existingUser);
            context.SaveChanges();
        }


        [Fact]
        public async Task When_IsUniqueEmailAsyncCalledWithExistingEmail_ShouldReturnFalseAsync()
        {
            var repository = new UserRepositoryAsync(context);

            var result = await repository.IsUniqueEmailAsync(existingUser.Email);
            Assert.False(result);
        }


        [Fact]
        public async Task When_IsUniqueEmailAsyncCalledWithNotExistingEmail_ShouldReturnTrueAsync()
        {
            var repository = new UserRepositoryAsync(context);

            var result = await repository.IsUniqueEmailAsync(_fixture.Create<string>());
            Assert.True(result);
        }
    }
}