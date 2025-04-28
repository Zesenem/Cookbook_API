using CookbookAPI.Domain;

namespace CookbookAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> RegisterAsync(User user);
        Task<User> UpdateAsync(User user);
        Task RemoveAsync(int id);

        Task<User> GetUserProfileAsync(int userId);
        Task AddFavoriteRecipeAsync(int userId, int recipeId);
        Task<List<Recipe>> GetUserFavoriteRecipesAsync(int userId);

        Task BlockUserAsync(int id);
        Task UnblockUserAsync(int id);

        Task<User> AuthenticateAsync(string email, string password);

        bool VerifyPassword(string Password, string HashedPassword);


    }
}
