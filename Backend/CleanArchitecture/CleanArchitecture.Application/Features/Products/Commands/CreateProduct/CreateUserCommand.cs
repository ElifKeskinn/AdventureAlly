using AutoMapper;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Wrappers;
using CleanArchitecture.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Features.Users.Commands.CreateProduct
{
    public partial class CreateUserCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateUserCommand, Response<int>>
    {
        private readonly IUserRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(IUserRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<User>(request);
            await _productRepository.AddAsync(product);
            return new Response<int>(product.Id);
        }
    }
}
