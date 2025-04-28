using CookbookAPI.Domain;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<Favorite>> GetAllAsync();
        Task<Favorite> GetByIdAsync(int id);
        Task AddAsync(Favorite favorite);
        Task UpdateAsync(Favorite favorite);
        Task RemoveAsync(int id);
        Task<bool> GetAnyAsync(int id);
        Task<Favorite> GetByUserAndRecipeAsync(int userId, int recipeId);
    }
}
