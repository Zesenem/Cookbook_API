using System.Threading.Tasks;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Implementations;
using Moq;
using Xunit;

namespace CookbookAPI.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var expectedUser = new User { Id = 1, Name = "Test User" };
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(1)).Returns(Task.FromResult(expectedUser));

            // Act
            var result = await _userService.GetByIdAsync(1);

            // Assert
            Assert.Equal(expectedUser, result);
        }

    }
}
