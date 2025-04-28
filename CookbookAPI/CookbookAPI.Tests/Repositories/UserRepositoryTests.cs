using System;
using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CookbookAPI.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private async Task<CookbookApiDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<CookbookApiDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            var databaseContext = new CookbookApiDbContext(options, configuration);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        [Fact]
        public async Task AddAsync_ShouldAddUserToDatabase()
        {
            using (var context = await GetDatabaseContext())
            {
                // Clear any existing users
                context.Users.RemoveRange(context.Users);
                await context.SaveChangesAsync();

                var repository = new UserRepository(context);
                var user = new User { Name = "Test User", Email = "test@example.com" };

                var result = await repository.AddAsync(user);

                Assert.NotEqual(0, result.Id);
                Assert.Equal(1, await context.Users.CountAsync());
            }
        }

        [Fact]
        public async Task GetByEmailAsync_ShouldReturnCorrectUser()
        {
            using (var context = await GetDatabaseContext())
            {
                var repository = new UserRepository(context);
                var user = new User { Name = "Test User", Email = "test@example.com" };
                context.Users.Add(user);
                await context.SaveChangesAsync();

                var result = await repository.GetByEmailAsync("test@example.com");

                Assert.NotNull(result);
                Assert.Equal(user.Name, result.Name);
            }
        }
    }
}
