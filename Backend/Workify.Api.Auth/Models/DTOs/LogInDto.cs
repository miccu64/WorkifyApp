namespace Workify.Api.Auth.Models.DTOs
{
    public record LogInDto
    {
        public required string Login;

        public required string Password;
    }
}
