using CookbookAPI.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookbookAPI.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<List<Recipe>> GetAllAsync();
        Task<Recipe> GetByIdAsync(int id);
        Task<List<Recipe>> SearchRecipesAsync(string name, int? categoryId, string difficulty, int? duration);
        Task<Recipe> CreateAsync(Recipe recipe);
        Task ApproveRecipeAsync(int recipeId);
        Task<Recipe> UpdateAsync(Recipe recipe);
        Task RemoveAsync(int id);
    }
}
