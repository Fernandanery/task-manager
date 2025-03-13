using FluentValidation;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.Application.Validators
{
    public class TaskValidator : AbstractValidator<CreateTaskDto>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(ValidationMessages.TitleRequired)
                .MaximumLength(100).WithMessage(ValidationMessages.TitleTooLong);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(ValidationMessages.DescriptionRequired)
                .MaximumLength(500).WithMessage(ValidationMessages.DescriptionTooLong);

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage(ValidationMessages.CreatedAtRequired)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ValidationMessages.InvalidCreatedAt);
        }
    }
}
