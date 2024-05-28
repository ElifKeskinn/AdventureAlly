using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Wrappers;
using MediatR;
using MongoDB.Bson;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Features.Users.Commands.DeleteUserById
{
    public class DeleteUserByIdCommand : IRequest<Response<ObjectId>>
    {
        public ObjectId Id { get; set; }
        public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, Response<ObjectId>>
        {
            private readonly IUserRepositoryAsync _userRepository;
            public DeleteUserByIdCommandHandler(IUserRepositoryAsync userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<Response<ObjectId>> Handle(DeleteUserByIdCommand command, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(command.Id);
                if (user == null) throw new ApiException($"User Not Found.");
                await _userRepository.DeleteAsync(user);
                return new Response<ObjectId>(user.Id);
            }
        }
    }
}
