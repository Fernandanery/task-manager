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
                .NotEmpty().WithMessage(Miscellaneous.NameRequired)
                .MinimumLength(3).WithMessage(Miscellaneous.NameTooShort)
                .MaximumLength(50).WithMessage(Miscellaneous.NameTooLong);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(Miscellaneous.EmailRequired)
                .EmailAddress().WithMessage(Miscellaneous.InvalidEmail);
        }
    }
}