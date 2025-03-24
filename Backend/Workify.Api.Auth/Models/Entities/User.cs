using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Workify.Api.Auth.Models.Entities
{
    internal class User
    {
        public required int Id { get; set; }
        public required string Login { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
    }

    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Login).IsRequired().HasMaxLength(32);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(255);
            builder.Property(e => e.HashedPassword).IsRequired().HasMaxLength(255);
        }
    }
}
