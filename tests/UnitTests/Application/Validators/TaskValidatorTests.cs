using FluentValidation.TestHelper;
using TaskManager.Application.Validators;
using TaskManager.SharedKernel.Constants;

namespace TaskManager.UnitTests.Application.Validators
{
    public class TaskValidatorTests
    {
        private readonly TaskValidator _validator;

        public TaskValidatorTests()
        {
            _validator = new TaskValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            var model = new CreateTaskDto { Title = "", Description = "Valid Description", CreatedAt = DateTime.UtcNow };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage(Miscellaneous.TitleRequired);
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Too_Long()
        {
            var model = new CreateTaskDto { Title = new string('A', 101), Description = "Valid Description", CreatedAt = DateTime.UtcNow };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Title)
                  .WithErrorMessage(Miscellaneous.TitleTooLong);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            var model = new CreateTaskDto { Title = "Valid Title", Description = "", CreatedAt = DateTime.UtcNow };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage(Miscellaneous.DescriptionRequired);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Too_Long()
        {
            var model = new CreateTaskDto { Title = "Valid Title", Description = new string('B', 501), CreatedAt = DateTime.UtcNow };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description)
                  .WithErrorMessage(Miscellaneous.DescriptionTooLong);
        }

        [Fact]
        public void Should_Have_Error_When_CreatedAt_Is_Empty()
        {
            var model = new CreateTaskDto { Title = "Valid Title", Description = "Valid Description", CreatedAt = default };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt)
                  .WithErrorMessage(Miscellaneous.CreatedAtRequired);
        }

        [Fact]
        public void Should_Have_Error_When_CreatedAt_Is_In_The_Future()
        {
            var model = new CreateTaskDto { Title = "Valid Title", Description = "Valid Description", CreatedAt = DateTime.UtcNow.AddMinutes(10) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.CreatedAt)
                  .WithErrorMessage(Miscellaneous.InvalidCreatedAt);
        }

        [Fact]
        public void Should_Not_Have_Errors_When_Model_Is_Valid()
        {
            // Arrange
            var model = new CreateTaskDto
            {
                Title = "Título Válido",
                Description = "Descrição válida da tarefa.",
                CreatedAt = DateTime.UtcNow.AddSeconds(-1)
            };

            // Act
            var result = _validator.TestValidate(model);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}