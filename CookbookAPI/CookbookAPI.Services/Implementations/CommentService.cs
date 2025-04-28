using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Interfaces;

namespace CookbookAPI.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly CookbookApiDbContext _cookbookApiDbContext;

        public CommentService(ICommentRepository commentRepository, CookbookApiDbContext cookbookApiDbContext)
        {
            _commentRepository = commentRepository;
            _cookbookApiDbContext = cookbookApiDbContext;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _commentRepository.AddAsync(comment);
            await _cookbookApiDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task RemoveAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment != null)
            {
                await _commentRepository.RemoveAsync(id);
                await _cookbookApiDbContext.SaveChangesAsync();
            }
        }
    }
}
