using System.ComponentModel.DataAnnotations;

namespace Workify.Api.Auth.Models.DTOs
{
    public record RegisterDto
    {
        [MinLength(4), MaxLength(32)]
        public required string Login;

        [MinLength(8), MaxLength(32)]
        public required string Password;

        [EmailAddress, MaxLength(255)]
        public required string Email;
    }
}
