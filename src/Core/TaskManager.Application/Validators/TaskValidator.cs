using FluentValidation;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Validators
{
    public class TaskValidator : AbstractValidator<UpdateTaskDto>
    {
        public TaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("O título é obrigatório.")
                .MaximumLength(200).WithMessage("O título deve ter no máximo 200 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.");

            RuleFor(x => x.IsCompleted)
                .NotNull().WithMessage("O status da tarefa é obrigatório.");
        }
    }
}
