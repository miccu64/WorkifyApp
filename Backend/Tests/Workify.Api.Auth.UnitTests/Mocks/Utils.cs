using Microsoft.EntityFrameworkCore;
using Moq;
using Workify.Api.Auth.Database;

namespace Workify.Api.Auth.UnitTests.Mocks
{
    internal class AuthDbContextMock : AuthDbContext
    {
        public AuthDbContextMock()
            : base(new Mock<DbContextOptions<AuthDbContext>>().Object) { }
    }
}
