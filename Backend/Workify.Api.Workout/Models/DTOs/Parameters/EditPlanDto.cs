using FluentValidation;

namespace Workify.Api.Workout.Models.DTOs.Parameters
{
    public record EditPlanDto(string Name, string? Description, IEnumerable<int> ExercisesIds);

    internal class EditPlanDtoValidator : AbstractValidator<EditPlanDto>
    {
        public EditPlanDtoValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty();
            RuleFor(dto => dto.ExercisesIds).NotNull();
        }
    }
}
