namespace Workify.Api.Auth.Models.DTOs
{
    public record LogInDto
    {
        public required string Login { get; init; }

        public required string Password { get; init; }
    }
}
