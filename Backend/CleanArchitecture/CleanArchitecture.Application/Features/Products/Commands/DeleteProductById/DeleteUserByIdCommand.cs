using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Features.Users.Commands.DeleteProductById
{
    public class DeleteUserByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, Response<int>>
        {
            private readonly IUserRepositoryAsync _userRepository;
            public DeleteUserByIdCommandHandler(IUserRepositoryAsync productRepository)
            {
                _userRepository = productRepository;
            }
            public async Task<Response<int>> Handle(DeleteUserByIdCommand command, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(command.Id);
                if (user == null) throw new ApiException($"User Not Found.");
                await _userRepository.DeleteAsync(user);
                return new Response<int>(user.Id);
            }
        }
    }
}
