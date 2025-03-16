using Microsoft.EntityFrameworkCore;
using Moq;
using Workify.Api.Auth.Database;
using Workify.Api.Auth.Models.Entities;

namespace Workify.Api.Auth.UnitTests.Mocks
{
    internal static class AuthDbContextMock
    {
        public static Mock<IAuthDbContext> GetMockDbContext(ICollection<User>? users = null)
        {
            var mockDbContext = new Mock<IAuthDbContext>();
            var mockUserDbSet = GetMockDbSet(users ?? []);

            mockDbContext.Setup(db => db.Users).Returns(mockUserDbSet.Object);
            mockDbContext.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

            return mockDbContext;
        }

        private static Mock<DbSet<T>> GetMockDbSet<T>(ICollection<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);

            return mockSet;
        }
    }
}
