using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Xunit;

namespace CookbookAPI.Tests.Repositories
{
    public class CategoryRepositoryTests
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
            await databaseContext.Database.EnsureCreatedAsync();
            return databaseContext;
        }

        [Fact]
        public async Task AddAsync_ShouldAddCategoryToDatabase()
        {
            using (var context = await GetDatabaseContext())
            {
                // Clear any existing categories
                context.Categories.RemoveRange(context.Categories);
                await context.SaveChangesAsync();

                var repository = new CategoryRepository(context);
                var category = new Category { Name = "Test Category" };

                var result = await repository.AddAsync(category);

                Assert.NotEqual(0, result.Id);
                Assert.Equal(1, await context.Categories.CountAsync());
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            using (var context = await GetDatabaseContext())
            {
                // Clear any existing categories
                context.Categories.RemoveRange(context.Categories);
                await context.SaveChangesAsync();

                var repository = new CategoryRepository(context);
                await context.Categories.AddRangeAsync(
                    new Category { Name = "Category 1" },
                    new Category { Name = "Category 2" }
                );
                await context.SaveChangesAsync();

                var result = await repository.GetAllAsync();

                Assert.Equal(2, result.Count);
                Assert.Equal(2, context.Categories.Count());
            }
        }
    }
}