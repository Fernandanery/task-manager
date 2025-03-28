using FluentValidation;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.Application.Validators
{
    public class TaskValidator : AbstractValidator<CreateTaskDto>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(Miscellaneous.TitleRequired)
                .MaximumLength(100).WithMessage(Miscellaneous.TitleTooLong);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(Miscellaneous.DescriptionRequired)
                .MaximumLength(500).WithMessage(Miscellaneous.DescriptionTooLong);

            RuleFor(x => x.CreatedAt)
                .NotEmpty().WithMessage(Miscellaneous.CreatedAtRequired)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage(Miscellaneous.InvalidCreatedAt);
        }
    }
}
