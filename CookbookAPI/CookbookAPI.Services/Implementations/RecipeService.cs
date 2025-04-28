using CookbookAPI.Domain;
using CookbookAPI.Services.Interfaces;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Data.Context;

namespace CookbookAPI.Services.Implementations
{
    public class RecipeService : IRecipeService
    {
        private readonly CookbookApiDbContext _cookbookApiDbContext;
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository, CookbookApiDbContext cookbookApiDbContext)
        {
            _cookbookApiDbContext = cookbookApiDbContext;
            _recipeRepository = recipeRepository;
        }

        public async Task<List<Recipe>> GetAllAsync()
        {
            return await _recipeRepository.GetAllAsync();
        }

        public async Task<Recipe> GetByIdAsync(int id)
        {
            return await _recipeRepository.GetByIdAsync(id);
        }

        public async Task<List<Recipe>> SearchRecipesAsync(string name, int? categoryId, string difficulty, int? duration)
        {
            return await _recipeRepository.SearchRecipesAsync(name, categoryId, difficulty, duration);
        }

        public async Task<Recipe> CreateAsync(Recipe recipe)
        {
            recipe.SubmissionDate = DateTime.UtcNow;
            await _recipeRepository.AddAsync(recipe);
            await _cookbookApiDbContext.SaveChangesAsync();
            return recipe;
        }

        public async Task ApproveRecipeAsync(int id)
        {
            await _recipeRepository.ApproveRecipeAsync(id);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<Recipe> UpdateAsync(Recipe recipe)
        {
            recipe = await _recipeRepository.UpdateAsync(recipe);
            await _cookbookApiDbContext.SaveChangesAsync();
            return recipe;
        }

        public async Task RemoveAsync(int id)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id);
            if (recipe != null)
            {
                await _recipeRepository.RemoveAsync(id);
                await _cookbookApiDbContext.SaveChangesAsync(); 
            }
        }
    }
}
