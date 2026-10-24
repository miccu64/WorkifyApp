using FluentValidation;

namespace Workify.Api.Auth.Models.DTOs
{
    public record LogInDto(string Login, string Password);

    public class LogInDtoValidator : AbstractValidator<LogInDto>
    {
        public LogInDtoValidator()
        {
            RuleFor(dto => dto.Login).NotEmpty();
            RuleFor(dto => dto.Password).NotEmpty();
        }
    }
}
