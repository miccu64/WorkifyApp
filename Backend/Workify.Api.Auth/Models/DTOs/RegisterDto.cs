using FluentValidation;

namespace Workify.Api.Auth.Models.DTOs
{
    public record RegisterDto
    {
        public required string Login;
        public required string Password;
        public required string Email;
    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(dto => dto.Login).MinimumLength(4).MaximumLength(32);
            RuleFor(dto => dto.Password).MinimumLength(8).MaximumLength(255);
            RuleFor(dto => dto.Email).EmailAddress();
        }
    }
}
