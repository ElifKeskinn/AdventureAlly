using AutoFixture;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
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
        private readonly IMongoCollection<User> userCollection;

        public UserRepositoryTest() {

            this._fixture = new Fixture();
            this._fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            this._fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            this.existingUser = _fixture.Create<User>();
            _dateTimeService = new Mock<IDateTimeService>();
            _authenticatedUserService = new Mock<IAuthenticatedUserService>();


            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("testdb");
            userCollection = database.GetCollection<User>("users");
            /*  var database = new Mock<IMongoDatabase>();
              var userCollection = new Mock<IMongoCollection<User>>();
              // Mock veritabaný ve koleksiyonun ayarlanmasý
              database.Setup(db => db.GetCollection<User>(It.IsAny<string>(), null))
                      .Returns(userCollection.Object);*/

            /* var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseInMemoryDatabase("TestDatabase");*/

            //context = new ApplicationDbContext(database.Object, _dateTimeService.Object, _authenticatedUserService.Object);
            context = new ApplicationDbContext(null, _dateTimeService.Object, _authenticatedUserService.Object);
            userCollection.InsertOne(existingUser);
            context.AddAsync(existingUser).Wait();  // AddAsync metodunu kullanarak kullanýcýyý ekliyoruz
           // context = new ApplicationDbContext(optionsBuilder.Options, _dateTimeService.Object, _authenticatedUserService.Object);

          //  context.Users.InsertOne(existingUser);
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

