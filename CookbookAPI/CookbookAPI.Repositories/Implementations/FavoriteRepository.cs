using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories.Implementations
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly CookbookApiDbContext _cookbookApiDbContext;
        private readonly DbSet<Favorite> _dbSet;

        public FavoriteRepository(CookbookApiDbContext cookbookApiDbContext)
        {
            _cookbookApiDbContext = cookbookApiDbContext;
            _dbSet = cookbookApiDbContext.Set<Favorite>();
        }

        public async Task<List<Favorite>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Favorite> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"Favorite with ID {id} not found.");
        }

        public async Task<Favorite> GetByUserAndRecipeAsync(int userId, int recipeId)
        {
            return await _dbSet.FirstOrDefaultAsync(f => f.UserId == userId && f.RecipeId == recipeId);
        }

        public async Task AddAsync(Favorite favorite)
        {
            await _dbSet.AddAsync(favorite);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Favorite favorite)
        {
            _dbSet.Update(favorite);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(int id)
        {
            var favorite = await GetByIdAsync(id);
            _dbSet.Remove(favorite);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<bool> GetAnyAsync(int id)
        {
            return await _dbSet.AnyAsync(f => f.Id == id);
        }
    }
}
