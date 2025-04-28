using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CookbookApiDbContext _cookbookApiDbContext;
        private readonly DbSet<User> _dbSet;

        public UserRepository(CookbookApiDbContext cookbookApiDbContext)
        {
            _cookbookApiDbContext = cookbookApiDbContext;
            _dbSet = _cookbookApiDbContext.Set<User>();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(user => user.Id == id)
                ?? throw new KeyNotFoundException($"User with ID {id} not found.");
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _cookbookApiDbContext.Users.Include(u => u.FavoriteRecipes).FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        public async Task AddFavoriteRecipeAsync(int userId, int recipeId)
        {
            var userExists = await _cookbookApiDbContext.Users.AnyAsync(u => u.Id == userId);
            var recipeExists = await _cookbookApiDbContext.Recipes.AnyAsync(r => r.Id == recipeId);

            if (!userExists || !recipeExists)
                throw new KeyNotFoundException("User or Recipe not found.");

            var favoriteExists = await _cookbookApiDbContext.Favorites
                            .AnyAsync(f => f.UserId == userId && f.RecipeId == recipeId);
            if (favoriteExists)
                throw new InvalidOperationException("Recipe is already in favorites.");

            var favorite = new Favorite
            {
                UserId = userId,
                RecipeId = recipeId
            };

            _cookbookApiDbContext.Favorites.Add(favorite);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<List<Recipe>> GetFavoriteRecipesAsync(int userId)
        {
            return await _cookbookApiDbContext.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Recipe)
                .ToListAsync();
        }

        public async Task BlockUserAsync(int id)
        {
            var user = await GetByIdAsync(id);
            user.IsBlocked = true;
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task UnblockUserAsync(int id)
        {
            var user = await GetByIdAsync(id);
            user.IsBlocked = false;
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(user => user.Email == email)
                ?? throw new KeyNotFoundException($"User with email {email} not found.");
        }

        public async Task<User> AddAsync(User user)
        {

            await _dbSet.AddAsync(user);
            await _cookbookApiDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbSet.Update(user);
            await _cookbookApiDbContext.SaveChangesAsync();
            return user;
        }

        public async Task RemoveAsync(int id)
        {
            var user = await GetByIdAsync(id);
            _dbSet.Remove(user);
            await _cookbookApiDbContext.SaveChangesAsync();
        }

        public async Task<bool> GetAnyAsync(int id)
        {
            return await _dbSet.AnyAsync(user => user.Id == id);
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(user => user.Email == email);
        }
    }
}
