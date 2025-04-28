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
    public class CommentRepositoryTests
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
        public async Task AddAsync_ShouldAddCommentToDatabase()
        {
            using (var context = await GetDatabaseContext())
            {
                context.Comments.RemoveRange(context.Comments);
                await context.SaveChangesAsync();

                var repository = new CommentRepository(context);
                var comment = new Comment { Content = "Test Comment", RecipeId = 1, UserId = 1 };

                var result = await repository.AddAsync(comment);

                Assert.NotEqual(0, result.Id);
                Assert.Equal(1, await context.Comments.CountAsync());
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectComment()
        {
            using (var context = await GetDatabaseContext())
            {
                context.Comments.RemoveRange(context.Comments);
                await context.SaveChangesAsync();

                var repository = new CommentRepository(context);
                var comment = new Comment { Content = "Test Comment", RecipeId = 1, UserId = 1 };
                await context.Comments.AddAsync(comment);
                await context.SaveChangesAsync();

                var result = await repository.GetByIdAsync(comment.Id);

                Assert.NotNull(result);
                Assert.Equal(comment.Content, result.Content);
                Assert.Equal(comment.RecipeId, result.RecipeId);
                Assert.Equal(comment.UserId, result.UserId);
            }
        }
    }
}
