using FluentValidation;
using TaskManager.Application.DTOs;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.Application.Validators
{
    public class UserValidator : AbstractValidator<CreateUserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.NameRequired)
                .MinimumLength(3).WithMessage(ValidationMessages.NameTooShort)
                .MaximumLength(50).WithMessage(ValidationMessages.NameTooLong);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.EmailRequired)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);
        }
    }
}
