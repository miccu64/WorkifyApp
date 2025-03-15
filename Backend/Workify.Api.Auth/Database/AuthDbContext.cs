using Microsoft.EntityFrameworkCore;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Database
{
    internal class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
