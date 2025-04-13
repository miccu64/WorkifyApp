using FluentValidation;

namespace Workify.Api.ExerciseStat.Models.DTOs.Parameters
{
    internal record CreateEditStatDto(DateTime Time, double Weight, int Reps, string? Note);

    internal class CreateEditStatDtoValidator : AbstractValidator<CreateEditStatDto>
    {
        public CreateEditStatDtoValidator()
        {
            RuleFor(dto => dto.Time).GreaterThan(DateTime.MinValue);
            RuleFor(dto => dto.Weight).GreaterThanOrEqualTo(0);
            RuleFor(dto => dto.Reps).GreaterThan(0);
        }
    }
}
