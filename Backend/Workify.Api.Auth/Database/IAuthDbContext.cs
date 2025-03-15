using Microsoft.EntityFrameworkCore;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.Database
{
    internal interface IAuthDbContext : IDisposable
    {
        public DbSet<User> Users { get; set; }

        public Task<int> SaveChangesAsync();
    }
}
