using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CookbookApiDbContext _cookbookApiDbContext;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository(CookbookApiDbContext cookbookApiDbContext)
        {
            _cookbookApiDbContext = cookbookApiDbContext;
            _dbSet = cookbookApiDbContext.Set<Category>();
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        public async Task<Category> AddAsync(Category category)
        {
            await _dbSet.AddAsync(category);
            await _cookbookApiDbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            _dbSet.Update(category);
            await _cookbookApiDbContext.SaveChangesAsync();
            return category;
        }

        public async Task RemoveAsync(int id)
        {
            var category = await GetByIdAsync(id);
            _dbSet.Remove(category);
            await _cookbookApiDbContext.SaveChangesAsync();
        }
    }
}
