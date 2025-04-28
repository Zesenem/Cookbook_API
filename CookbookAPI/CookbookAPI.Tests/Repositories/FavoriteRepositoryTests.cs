using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CookbookAPI.Tests.Repositories
{
    public class FavoriteRepositoryTests
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
        public async Task AddAsync_ShouldAddFavoriteToDatabase()
        {
            using (var context = await GetDatabaseContext())
            {
                context.Favorites.RemoveRange(context.Favorites);
                await context.SaveChangesAsync();

                var repository = new FavoriteRepository(context);
                var favorite = new Favorite { RecipeId = 1, UserId = 1 };

                var result = repository.AddAsync(favorite);

                Assert.NotEqual(0, result.Id);
                Assert.Equal(1, await context.Favorites.CountAsync());
            }
        }

        [Fact]
        public async Task GetByUserAndRecipeAsync_ShouldReturnCorrectFavorite()
        {
            using (var context = await GetDatabaseContext())
            {
                context.Favorites.RemoveRange(context.Favorites);
                await context.SaveChangesAsync();

                var repository = new FavoriteRepository(context);
                var favorite = new Favorite { RecipeId = 1, UserId = 1 };
                await context.Favorites.AddAsync(favorite);
                await context.SaveChangesAsync();

                var result = await repository.GetByUserAndRecipeAsync(1, 1);

                Assert.NotNull(result);
                Assert.Equal(favorite.RecipeId, result.RecipeId);
                Assert.Equal(favorite.UserId, result.UserId);
            }
        }
    }
}
