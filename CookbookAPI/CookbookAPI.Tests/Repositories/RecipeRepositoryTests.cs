using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CookbookAPI.Tests.Repositories
{
    public class RecipeRepositoryTests
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
        public async Task AddAsync_ShouldAddRecipeToDatabase()
        {
            using (var context = await GetDatabaseContext())
            {
                // Clear any existing recipes
                context.Recipes.RemoveRange(context.Recipes);
                await context.SaveChangesAsync();

                var repository = new RecipeRepository(context);
                var recipe = new Recipe { Name = "Test Recipe", Instructions = "Test Instructions" };

                var result = repository.AddAsync(recipe);

                Assert.NotEqual(0, result.Id);
                Assert.Equal(1, await context.Recipes.CountAsync());
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectRecipe()
        {
            using (var context = await GetDatabaseContext())
            {
                var repository = new RecipeRepository(context);
                var recipe = new Recipe { Name = "Test Recipe", Instructions = "Test Instructions" };
                context.Recipes.Add(recipe);
                await context.SaveChangesAsync();

                var result = await repository.GetByIdAsync(recipe.Id);

                Assert.NotNull(result);
                Assert.Equal(recipe.Name, result.Name);
            }
        }
    }
}
