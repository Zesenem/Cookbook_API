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
    public class RatingRepositoryTests
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
        public async Task AddAsync_ShouldAddRatingToDatabase()
        {
            using (var context = await GetDatabaseContext())
            {
                context.Ratings.RemoveRange(context.Ratings);
                await context.SaveChangesAsync();

                var repository = new RatingRepository(context);
                var rating = new Rating { Stars = 5, RecipeId = 1, UserId = 1 };

                var result = await repository.AddAsync(rating);

                Assert.NotEqual(0, result.Id);
                Assert.Equal(1, await context.Ratings.CountAsync());
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectRating()
        {
            using (var context = await GetDatabaseContext())
            {
                context.Ratings.RemoveRange(context.Ratings);
                await context.SaveChangesAsync();

                var repository = new RatingRepository(context);
                var rating = new Rating { Stars = 4, RecipeId = 1, UserId = 1 };
                await context.Ratings.AddAsync(rating);
                await context.SaveChangesAsync();

                var result = await repository.GetByIdAsync(rating.Id);

                Assert.NotNull(result);
                Assert.Equal(rating.Stars, result.Stars);
                Assert.Equal(rating.RecipeId, result.RecipeId);
                Assert.Equal(rating.UserId, result.UserId);
            }
        }
    }
}