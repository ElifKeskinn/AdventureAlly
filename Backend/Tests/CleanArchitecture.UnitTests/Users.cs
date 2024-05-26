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

namespace CleanArchitecture.UnitTests
{
    public class Users
    {
        private readonly Fixture fixture;
        private readonly Mock<IUserRepositoryAsync> userRepositoryAsync;

        public Users()
        {

            fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            userRepositoryAsync = new Mock<IUserRepositoryAsync>();


        }


        [Fact]
        public void When_UpdateUserCommandHandlerInvoked_WithNotExistingUser_ShouldThrowEntityNotFoundException()
        {
            userRepositoryAsync
                .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            var updateUserCommandHandler = new UpdateUserCommandHandler(userRepositoryAsync.Object);

            var command = new UpdateUserCommand();
            var cancellationToken = new CancellationToken();
            Assert.ThrowsAsync<EntityNotFoundException>(() => updateUserCommandHandler.Handle(command, cancellationToken));
        }

        [Fact]
        public void When_UpdateUserCommandHandlerInvoked_WithNotUniqueEmail_ShouldThrowEmailIsNotUniqueException()
        {
            this.userRepositoryAsync
                .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(this.fixture.Create<User>());

            this.userRepositoryAsync
                .Setup(pr => pr.IsUniqueEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var updateUserCommandHandler = new UpdateUserCommandHandler(this.userRepositoryAsync.Object);

            var command = this.fixture.Create<UpdateUserCommand>();
            var cancellationToken = this.fixture.Create<CancellationToken>();

            Assert.ThrowsAsync<EmailIsNotUniqueException>(() => updateUserCommandHandler.Handle(command, cancellationToken));
        }

        [Fact]
        public async Task When_UpdateUserCommandHandlerInvoked_ShouldReturnUserId()
        {
            var user = fixture.Build<User>().Create();

            var userId = user.Id;

            this.userRepositoryAsync
              .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
              .ReturnsAsync(user);

            this.userRepositoryAsync
                .Setup(pr => pr.IsUniqueEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var updateUserCommandHandler = new UpdateUserCommandHandler(this.userRepositoryAsync.Object);

            var command = new UpdateUserCommand { Id = userId };
            var cancellationToken = new CancellationToken();


            var result = await updateUserCommandHandler.Handle(command, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Data);

        }

        [Fact]
        public async Task When_DeleteUserByIdCommandInvoked_ShouldDeleteUserReturnUserID()
        {
            var user = fixture.Create<User>();
            var userId = user.Id;


            this.userRepositoryAsync
             .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
             .ReturnsAsync(user);


            this.userRepositoryAsync
            .Setup(pr => pr.IsUniqueEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        var deleteUserCommandHandler = new DeleteUserByIdCommandHandler(this.userRepositoryAsync.Object);

            var command = new DeleteUserByIdCommand { Id = userId };
            var cancellationToken = new CancellationToken();

            var result = await deleteUserCommandHandler.Handle(command, cancellationToken);

        Assert.NotNull(result);
            Assert.Equal(command.Id, result.Data);
        }


    }

   
}