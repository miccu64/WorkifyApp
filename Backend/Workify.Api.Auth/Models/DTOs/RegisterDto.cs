using System.ComponentModel.DataAnnotations;

namespace Workify.Api.Auth.Models.DTOs
{
    public record RegisterDto
    {
        [MinLength(4), MaxLength(32)]
        public required string Login { get; init; }

        [MinLength(8), MaxLength(32)]
        public required string Password { get; init; }

        [EmailAddress]
        public required string Email { get; init; }
    }
}
