using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories.Implementations
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly CookbookApiDbContext _cookbookApiDbContext;
        private readonly DbSet<Recipe> _dbSet;

        public RecipeRepository(CookbookApiDbContext cookbookApiDbContext)
        {
            _cookbookApiDbContext = cookbookApiDbContext;
            _dbSet = cookbookApiDbContext.Set<Recipe>();
        }

        public async Task<List<Recipe>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Recipe> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id)
                ?? throw new KeyNotFoundException($"Recipe with ID {id} not found.");
        }

        public async Task<List<Recipe>> SearchRecipesAsync(string name, int? categoryId, string difficulty, int? duration)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.Name.Contains(name));

            if (categoryId.HasValue)
                query = query.Where(r => r.CategoryId == categoryId);

            if (!string.IsNullOrEmpty(difficulty))
                query = query.Where(r => r.Difficulty.ToLower() == difficulty.ToLower());

            if (duration.HasValue)
                query = query.Where(r => r.Duration <= duration);

            return await query.ToListAsync();
        }

        public async Task AddAsync(Recipe recipe)
        {
            await _dbSet.AddAsync(recipe);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<bool> GetAnyAsync(int id)
        {
            return await _dbSet.AnyAsync(recipe => recipe.Id == id);
        }

        public async Task ApproveRecipeAsync(int id)
        {
            var recipe = await GetByIdAsync(id);
            recipe.IsApproved = true;
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<List<Recipe>> GetByNameAsync(string name)
        {
            return await _dbSet.Where(recipe => recipe.Name.Contains(name)).ToListAsync();
        }

        public async Task<Recipe> UpdateAsync(Recipe recipe)
        {
            _dbSet.Update(recipe);
            await _cookbookApiDbContext.SaveChangesAsync();
            return recipe;
        }

        public async Task RemoveAsync(int id)
        {
            var recipe = await GetByIdAsync(id);
            _dbSet.Remove(recipe);
            await _cookbookApiDbContext.SaveChangesAsync();
        }
    }
}
