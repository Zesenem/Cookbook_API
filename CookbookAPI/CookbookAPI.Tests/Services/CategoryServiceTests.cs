using System.Collections.Generic;
using System.Threading.Tasks;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Implementations;
using Moq;
using Xunit;

namespace CookbookAPI.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategories()
        {
            // Arrange
            var expectedCategories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).Returns(Task.FromResult(expectedCategories));

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            Assert.Equal(expectedCategories.Count, result.Count);
            Assert.Equal(expectedCategories, result);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedCategory()
        {
            // Arrange
            var newCategory = new Category { Name = "New Category" };
            var createdCategory = new Category { Id = 1, Name = "New Category" };
            _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>())).Returns(Task.FromResult(createdCategory));

            // Act
            var result = await _categoryService.CreateAsync(newCategory);

            // Assert
            Assert.Equal(createdCategory.Id, result.Id);
            Assert.Equal(createdCategory.Name, result.Name);
        }

        // Add more tests for other CategoryService methods
    }
}