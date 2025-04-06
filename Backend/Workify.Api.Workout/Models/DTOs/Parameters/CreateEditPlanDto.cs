using FluentValidation;

namespace Workify.Api.Workout.Models.DTOs.Parameters
{
    public record CreateEditPlanDto(string Name, string? Description, IEnumerable<int> ExercisesIds);

    internal class CreateEditPlanDtoValidator : AbstractValidator<CreateEditPlanDto>
    {
        public CreateEditPlanDtoValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().MaximumLength(255);
            RuleFor(dto => dto.Description).MaximumLength(1023);
            RuleFor(dto => dto.ExercisesIds).NotNull().ForEach(id => id.GreaterThan(0));
        }
    }
}
