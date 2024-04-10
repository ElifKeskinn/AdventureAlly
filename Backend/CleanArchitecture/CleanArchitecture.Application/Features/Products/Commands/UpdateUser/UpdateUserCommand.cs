using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Features.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<int>>
        {
            private readonly IUserRepositoryAsync _userRepository;
            public UpdateUserCommandHandler(IUserRepositoryAsync userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<Response<int>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(command.Id);

                if (user == null) throw new EntityNotFoundException("user", command.Id);

                var isUniqueBarcode = await _userRepository.IsUniqueBarcodeAsync(command.Barcode);

                if(!isUniqueBarcode) throw new BarcodeIsNotUniqueException(command.Barcode);

                user.Name = command.Name;
                user.Rate = command.Rate;
                user.Description = command.Description;
                await _userRepository.UpdateAsync(user);
                return new Response<int>(user.Id);
            }
        }
    }
}
