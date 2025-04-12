using FluentValidation;

namespace Workify.Api.Auth.Models.DTOs
{
    public record RegisterDto(string Login, string Password, string Email);

    internal class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(dto => dto.Login).NotEmpty().MinimumLength(4).MaximumLength(31);
            RuleFor(dto => dto.Password).NotEmpty().MinimumLength(8).MaximumLength(255);
            RuleFor(dto => dto.Email).NotEmpty().EmailAddress();
        }
    }
}
