using CleanArchitecture.Core.Interfaces.Repositories;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepositoryAsync userRepository;

        public CreateUserCommandValidator(IUserRepositoryAsync UserRepository)
        {
            this.userRepository = UserRepository;
            RuleFor(p => p.Email)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .NotNull()
               .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
               .EmailAddress().WithMessage("{PropertyName} is not a valid email address.")
               .MustAsync(IsUniqueEmail).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.Phone)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(11).WithMessage("{PropertyName} must not exceed 11 characters.");

            RuleFor(p => p.Interests)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.PreferredLanguage)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");


        }

        private async Task<bool> IsUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await userRepository.IsUniqueEmailAsync(email);
        }
    }
}
