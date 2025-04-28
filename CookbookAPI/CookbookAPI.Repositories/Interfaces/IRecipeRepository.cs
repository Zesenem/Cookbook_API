using CookbookAPI.Domain;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetAllAsync();
        Task<Recipe> GetByIdAsync(int id);
        Task<bool> GetAnyAsync(int id);
        Task<List<Recipe>> GetByNameAsync(string name);
        Task AddAsync(Recipe recipe);
        Task<Recipe> UpdateAsync(Recipe recipe);
        Task RemoveAsync(int id);
        Task<List<Recipe>> SearchRecipesAsync(string name, int? categoryId, string difficulty, int? duration);
        Task ApproveRecipeAsync(int id);

    }
}
