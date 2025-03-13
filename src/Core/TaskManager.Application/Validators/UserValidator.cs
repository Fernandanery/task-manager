using FluentValidation;
using TaskManager.Application.DTOs;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.Application.Validators
{
    public class UserValidator : AbstractValidator<UpdateUserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField)
                .MaximumLength(50).WithMessage(ValidationMessages.MaxLength);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(ValidationMessages.RequiredField)
                .EmailAddress().WithMessage(ValidationMessages.InvalidEmail);
        }
    }
}
