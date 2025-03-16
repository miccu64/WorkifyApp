using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Workify.Api.Auth.Models.Entities
{
    internal class User
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
    }

    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Login).IsRequired().HasMaxLength(32);
            builder.Property(user => user.Email).IsRequired().HasMaxLength(255);
            builder.Property(user => user.HashedPassword).IsRequired().HasMaxLength(255);
        }
    }
}
