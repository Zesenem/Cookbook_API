using System.Threading.Tasks;
using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Implementations;
using Moq;
using Xunit;

namespace CookbookAPI.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<CookbookApiDbContext> _mockDbContext;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockDbContext = new Mock<CookbookApiDbContext>();
            _commentService = new CommentService(_mockCommentRepository.Object, _mockDbContext.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedComment()
        {
            // Arrange
            var newComment = new Comment { Content = "New Comment", RecipeId = 1, UserId = 1 };
            _mockCommentRepository.Setup(repo => repo.AddAsync(It.IsAny<Comment>())).Returns(Task.FromResult(newComment));

            // Act
            var result = await _commentService.CreateAsync(newComment);

            // Assert
            Assert.Equal(newComment, result);
            _mockDbContext.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        // Add more tests for other CommentService methods
    }
}