using System.ComponentModel.DataAnnotations;

namespace Workify.Api.Auth.Models.DTOs
{
    public record RegisterDto
    {
        [MinLength(4), MaxLength(32)]
        public required string Login;

        [MinLength(8), MaxLength(32)]
        public required string Password;

        [EmailAddress]
        public required string Email;
    }
}
