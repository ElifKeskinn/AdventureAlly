using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Features.Users.Commands.DeleteProductById;
using CleanArchitecture.Core.Features.Users.Commands.UpdateProduct;
using CleanArchitecture.Core.Interfaces.Repositories;
using Moq;
using static CleanArchitecture.Core.Features.Users.Commands.DeleteProductById.DeleteUserByIdCommand;
using static CleanArchitecture.Core.Features.Users.Commands.UpdateProduct.UpdateUserCommand;

namespace CleanArchitecture.UnitTests
{
    public class Products
    {
        private readonly Fixture fixture;
        private readonly Mock<IUserRepositoryAsync> productRepositoryAsync;

        public Products()
        {
            fixture = new Fixture();
            productRepositoryAsync = new Mock<IUserRepositoryAsync>();
        }


        [Fact]
        public void When_UpdateProductCommandHandlerInvoked_WithNotExistingProduct_ShouldThrowEntityNotFoundException()
        {
            productRepositoryAsync
                .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            var updateProductCommandHandler = new UpdateProductCommandHandler(productRepositoryAsync.Object);

            var command = this.fixture.Create<UpdateUserCommand>();
            var cancellationToken = this.fixture.Create<CancellationToken>();

            Assert.ThrowsAsync<EntityNotFoundException>(() => updateProductCommandHandler.Handle(command, cancellationToken));
        }

        [Fact]
        public void When_UpdateProductCommandHandlerInvoked_WithNotUniqueBarcode_ShouldThrowBarcodeIsNotUniqueException()
        {
            this.productRepositoryAsync
                .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(this.fixture.Create<User>());

            this.productRepositoryAsync
                .Setup(pr => pr.IsUniqueBarcodeAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var updateProductCommandHandler = new UpdateProductCommandHandler(this.productRepositoryAsync.Object);

            var command = this.fixture.Create<UpdateUserCommand>();
            var cancellationToken = this.fixture.Create<CancellationToken>();

            Assert.ThrowsAsync<BarcodeIsNotUniqueException>(() => updateProductCommandHandler.Handle(command, cancellationToken));
        }

        [Fact]
        public async Task When_UpdateProductCommandHandlerInvoked_ShouldReturnProductId()
        {
            User product = this.fixture.Create<User>();
            this.fixture.Customize<UpdateUserCommand>(c => c.With(x => x.Id, product.Id));

            this.productRepositoryAsync
              .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
              .ReturnsAsync(product);

            this.productRepositoryAsync
                .Setup(pr => pr.IsUniqueBarcodeAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var updateProductCommandHandler = new UpdateProductCommandHandler(this.productRepositoryAsync.Object);

            var command = this.fixture.Create<UpdateUserCommand>();

            var cancellationToken = this.fixture.Create<CancellationToken>();

            var result = await updateProductCommandHandler.Handle(command, cancellationToken);

            Assert.NotNull(result);
            Assert.Equal(command.Id, result.Data);

        }

        [Fact]
        public class When_DeleteProductByIdCommandInvoked_ShouldDeleteProductReturnProductID()
        {
         User product = this.fixture.Create<User>();
            this.fixture.Customize<DeleteUserByIdCommand>(c => c.With(x => x.Id, product.Id));

            this.productRepositoryAsync
              .Setup(pr => pr.GetByIdAsync(It.IsAny<int>()))
              .ReturnsAsync(product);

            /*this.productRepositoryAsync
            .Setup(pr => pr.IsUniqueBarcodeAsync(It.IsAny<string>()))
            .ReturnsAsync(true);*/

        var deleteProductCommandHandler = new DeleteProductByIdCommandHandler(this.productRepositoryAsync.Object);

        var command = this.fixture.Create<DeleteUserByIdCommand>();

        var cancellationToken = this.fixture.Create<CancellationToken>();

        var result = await deleteProductCommandHandler.Handle(command, cancellationToken);

        Assert.NotNull(result);
            Assert.Equal(command.Id, result.Data);
}


    }

   
}