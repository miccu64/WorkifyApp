using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Database.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Login).IsRequired().HasMaxLength(255);
            builder.Property(user => user.HashedPassword).IsRequired().HasMaxLength(255);
        }
    }
}
