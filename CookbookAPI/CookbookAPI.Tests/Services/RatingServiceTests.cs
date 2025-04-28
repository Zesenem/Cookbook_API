using System;
using System.Collections.Generic;
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
    public class RatingServiceTests
    {
        private readonly Mock<IRatingRepository> _mockRatingRepository;
        private readonly Mock<CookbookApiDbContext> _mockDbContext;
        private readonly RatingService _ratingService;

        public RatingServiceTests()
        {
            _mockRatingRepository = new Mock<IRatingRepository>();
            _mockDbContext = new Mock<CookbookApiDbContext>();
            _ratingService = new RatingService(_mockRatingRepository.Object, _mockDbContext.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedRating()
        {
            // Arrange
            var newRating = new Rating { Stars = 5, RecipeId = 1, UserId = 1 };
            _mockRatingRepository.Setup(repo => repo.AddAsync(It.IsAny<Rating>()))
                .ReturnsAsync(newRating);

            // Act
            var result = await _ratingService.CreateAsync(newRating);

            // Assert
            Assert.Equal(newRating, result);
            _mockRatingRepository.Verify(repo => repo.AddAsync(It.IsAny<Rating>()), Times.Once);
        }

        [Fact]
        public async Task GetRatingStatsByRecipeId_ShouldReturnCorrectStats()
        {
            // Arrange
            var ratings = new List<Rating>
            {
                new Rating { Id = 1, Stars = 4, RecipeId = 1, UserId = 1 },
                new Rating { Id = 2, Stars = 5, RecipeId = 1, UserId = 2 },
                new Rating { Id = 3, Stars = 3, RecipeId = 1, UserId = 3 },
            };
            _mockRatingRepository.Setup(repo => repo.GetAllAsync()).Returns(Task.FromResult(ratings));

            // Act
            var result = await _ratingService.GetRatingStatsByRecipeId(1);

            // Assert
            Assert.Equal(4, result.AverageRating);
            Assert.Equal(3, result.Count);
        }
    }
}
