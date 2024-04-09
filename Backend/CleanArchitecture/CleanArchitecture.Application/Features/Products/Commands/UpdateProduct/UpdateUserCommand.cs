using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Features.Users.Commands.UpdateProduct
{
    public class UpdateUserCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateUserCommand, Response<int>>
        {
            private readonly IUserRepositoryAsync _productRepository;
            public UpdateProductCommandHandler(IUserRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<Response<int>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(command.Id);

                if (product == null) throw new EntityNotFoundException("product", command.Id);

                var isUniqueBarcode = await _productRepository.IsUniqueBarcodeAsync(command.Barcode);

                if(!isUniqueBarcode) throw new BarcodeIsNotUniqueException(command.Barcode);

                product.Name = command.Name;
                product.Rate = command.Rate;
                product.Description = command.Description;
                await _productRepository.UpdateAsync(product);
                return new Response<int>(product.Id);
            }
        }
    }
}
