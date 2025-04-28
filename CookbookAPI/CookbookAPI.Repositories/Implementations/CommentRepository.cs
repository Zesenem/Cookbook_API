using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CookbookApiDbContext _cookbookApiDbContext;
        private readonly DbSet<Comment> _dbSet;

        public CommentRepository(CookbookApiDbContext cookbookApiDbContext)
        {
            _cookbookApiDbContext = cookbookApiDbContext;
            _dbSet = cookbookApiDbContext.Set<Comment>();
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"Comment with ID {id} not found.");
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            await _dbSet.AddAsync(comment);
            await _cookbookApiDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            _dbSet.Update(comment);
            await _cookbookApiDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task RemoveAsync(int id)
        {
            var comment = await GetByIdAsync(id);
            _dbSet.Remove(comment);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<bool> GetAnyAsync(int id)
        {
            return await _dbSet.AnyAsync(c => c.Id == id);
        }
    }
}
