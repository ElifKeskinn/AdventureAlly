using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Wrappers;
using CleanArchitecture.Core.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Features.Users.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Response<User>>
    {
        public int Id { get; set; }
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Response<User>>
        {
            private readonly IUserRepositoryAsync _productRepository;
            public GetProductByIdQueryHandler(IUserRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<Response<User>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(query.Id);
                if (product == null) throw new ApiException($"User Not Found.");
                return new Response<User>(product);
            }
        }
    }
}
