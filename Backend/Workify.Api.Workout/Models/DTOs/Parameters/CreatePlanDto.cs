using FluentValidation;

namespace Workify.Api.Workout.Models.DTOs.Parameters
{
    public record CreatePlanDto(string Name, string? Description, IEnumerable<int> ExercisesIds);

    internal class CreatePlanDtoValidator : AbstractValidator<CreatePlanDto>
    {
        public CreatePlanDtoValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().MaximumLength(255);
            RuleFor(dto => dto.Description).MaximumLength(1023);
            RuleFor(dto => dto.ExercisesIds).NotNull();
        }
    }
}
