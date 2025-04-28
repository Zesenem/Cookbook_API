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
    public class RecipeServiceTests
    {
        private readonly Mock<IRecipeRepository> _mockRecipeRepository;
        private readonly Mock<CookbookApiDbContext> _mockDbContext;
        private readonly RecipeService _recipeService;

        public RecipeServiceTests()
        {
            _mockRecipeRepository = new Mock<IRecipeRepository>();
            _mockDbContext = new Mock<CookbookApiDbContext>();
            _recipeService = new RecipeService(_mockRecipeRepository.Object, _mockDbContext.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRecipes()
        {
            // Arrange
            var expectedRecipes = new List<Recipe>
            {
                new Recipe { Id = 1, Name = "Recipe 1" },
                new Recipe { Id = 2, Name = "Recipe 2" }
            };
            _mockRecipeRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedRecipes);

            // Act
            var result = await _recipeService.GetAllAsync();

            // Assert
            Assert.Equal(expectedRecipes.Count, result.Count);
            Assert.Equal(expectedRecipes, result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRecipe_WhenRecipeExists()
        {
            // Arrange
            var expectedRecipe = new Recipe { Id = 1, Name = "Test Recipe" };
            _mockRecipeRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(expectedRecipe);

            // Act
            var result = await _recipeService.GetByIdAsync(1);

            // Assert
            Assert.Equal(expectedRecipe, result);
        }

    }
}
