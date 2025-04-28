using CookbookAPI.Domain;

namespace CookbookAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task RemoveAsync(int id);
        Task<bool> GetAnyAsync(int id);

        Task<User> GetUserByIdAsync(int userId);
        Task AddFavoriteRecipeAsync(int userId, int recipeId);
        Task<List<Recipe>> GetFavoriteRecipesAsync(int userId);

        Task BlockUserAsync(int id);
        Task UnblockUserAsync(int id);
        Task<bool> UserExistsByEmailAsync(string email);
    }
}
