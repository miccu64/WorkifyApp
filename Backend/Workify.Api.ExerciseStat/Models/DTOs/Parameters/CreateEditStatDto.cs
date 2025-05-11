using FluentValidation;

namespace Workify.Api.ExerciseStat.Models.DTOs.Parameters
{
    public record CreateEditStatDto(DateTimeOffset Time, double Weight, int Reps, string? Note);

    internal class CreateEditStatDtoValidator : AbstractValidator<CreateEditStatDto>
    {
        public CreateEditStatDtoValidator()
        {
            RuleFor(dto => dto.Time).GreaterThan(DateTimeOffset.MinValue);
            RuleFor(dto => dto.Weight).GreaterThanOrEqualTo(0);
            RuleFor(dto => dto.Reps).GreaterThan(0);
        }
    }
}
