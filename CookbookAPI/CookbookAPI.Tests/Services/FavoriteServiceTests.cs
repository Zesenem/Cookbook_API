using System.Threading.Tasks;
using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CookbookAPI.Tests.Services
{
    public class FavoriteServiceTests
    {
        private readonly Mock<IFavoriteRepository> _mockFavoriteRepository;
        private readonly Mock<CookbookApiDbContext> _mockDbContext;
        private readonly FavoriteService _favoriteService;

        public FavoriteServiceTests()
        {
            _mockFavoriteRepository = new Mock<IFavoriteRepository>();
            _mockDbContext = new Mock<CookbookApiDbContext>();
            _favoriteService = new FavoriteService(_mockFavoriteRepository.Object, _mockDbContext.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedFavorite()
        {
            // Arrange
            var newFavorite = new Favorite { RecipeId = 1, UserId = 1 };
            _mockFavoriteRepository.Setup(repo => repo.AddAsync(It.IsAny<Favorite>())).Returns(Task.CompletedTask);

            // Act
            var result = await _favoriteService.CreateAsync(newFavorite);

            // Assert
            Assert.Equal(newFavorite, result);
            _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_ShouldReturnTrueWhenFavoriteExists()
        {
            // Arrange
            var existingFavorite = new Favorite { Id = 1, RecipeId = 1, UserId = 1 };
            _mockFavoriteRepository.Setup(repo => repo.GetByUserAndRecipeAsync(1, 1)).Returns(Task.FromResult(existingFavorite));
            _mockFavoriteRepository.Setup(repo => repo.RemoveAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _favoriteService.RemoveAsync(1, 1);

            // Assert
            Assert.True(result);
            _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }
    }
}
