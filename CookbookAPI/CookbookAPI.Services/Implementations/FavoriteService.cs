using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Interfaces;

namespace CookbookAPI.Services.Implementations
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly CookbookApiDbContext _cookbookApiDbContext;

        public FavoriteService(IFavoriteRepository favoriteRepository, CookbookApiDbContext cookbookApiDbContext)
        {
            _favoriteRepository = favoriteRepository;
            _cookbookApiDbContext = cookbookApiDbContext;
        }

        public async Task<List<Favorite>> GetAllAsync()
        {
            return await _favoriteRepository.GetAllAsync();
        }

        public async Task<Favorite> GetByIdAsync(int id)
        {
            return await _favoriteRepository.GetByIdAsync(id);
        }

        public async Task<Favorite> CreateAsync(Favorite favorite)
        {
            await _favoriteRepository.AddAsync(favorite);
            await _cookbookApiDbContext.SaveChangesAsync();
            return favorite;
        }

        public async Task<bool> RemoveAsync(int userId, int recipeId)
        {
            var favorite = await _favoriteRepository.GetByUserAndRecipeAsync(userId, recipeId);
            if (favorite != null)
            {
                await _favoriteRepository.RemoveAsync(favorite.Id);
                await _cookbookApiDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
