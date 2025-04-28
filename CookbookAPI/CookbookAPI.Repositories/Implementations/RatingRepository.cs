using CookbookAPI.Data.Context;
using Microsoft.EntityFrameworkCore;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;

namespace CookbookAPI.Repositories.Implementations
{
    public class RatingRepository : IRatingRepository
    {
        private readonly CookbookApiDbContext _cookbookApiDbContext;
        private readonly DbSet<Rating> _dbSet;

        public RatingRepository(CookbookApiDbContext cookbookApiDbContext)
        {
            _cookbookApiDbContext = cookbookApiDbContext;
            _dbSet = cookbookApiDbContext.Set<Rating>();
        }

        public async Task<List<Rating>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Rating> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"Rating with ID {id} not found.");
        }

        public async Task<Rating> AddAsync(Rating rating)
        {
            await _dbSet.AddAsync(rating);
            await _cookbookApiDbContext.SaveChangesAsync();
            return rating;
        }

        public async Task<Rating> UpdateAsync(Rating rating)
        {
            _dbSet.Update(rating);
            await _cookbookApiDbContext.SaveChangesAsync();
            return rating;
        }

        public async Task RemoveAsync(int id)
        {
            var rating = await GetByIdAsync(id);
            _dbSet.Remove(rating);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<bool> GetAnyAsync(int id)
        {
            return await _dbSet.AnyAsync(r => r.Id == id);
        }
    }
}
