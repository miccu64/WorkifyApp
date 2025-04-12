using FluentValidation;
using Workify.Api.Workout.Models.Others;

namespace Workify.Api.Workout.Models.DTOs.Parameters
{
    internal record CreateEditExerciseDto(string Name, BodyPartEnum BodyPart, string? Description);

    internal class CreateEditExerciseDtoValidator : AbstractValidator<CreateEditExerciseDto>
    {
        public CreateEditExerciseDtoValidator()
        {
            RuleFor(dto => dto.Name).NotEmpty().MaximumLength(255);
            RuleFor(dto => dto.BodyPart).IsInEnum();
            RuleFor(dto => dto.Description).MaximumLength(1023);
        }
    }
}
