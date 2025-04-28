using CookbookAPI.Data.Context;
using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Interfaces;

namespace CookbookAPI.Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly CookbookApiDbContext _cookbookApiDbContext;

        public RatingService(IRatingRepository ratingRepository, CookbookApiDbContext cookbookApiDbContext)
        {
            _ratingRepository = ratingRepository;
            _cookbookApiDbContext = cookbookApiDbContext;
        }

        public async Task<List<Rating>> GetAllAsync()
        {
            return await _ratingRepository.GetAllAsync();
        }

        public async Task<Rating> GetByIdAsync(int id)
        {
            return await _ratingRepository.GetByIdAsync(id);
        }

        public async Task<Rating> CreateAsync(Rating rating)
        {
            await _ratingRepository.AddAsync(rating);
            await _cookbookApiDbContext.SaveChangesAsync();
            return rating;
        }

        public async Task<Rating> UpdateAsync(Rating rating)
        {
            var existingRating = await _ratingRepository.GetByIdAsync(rating.Id);
            if (existingRating == null)
            {
                throw new KeyNotFoundException("Rating not found");
            }

            existingRating.Stars = rating.Stars;
            existingRating.RecipeId = rating.RecipeId;
            existingRating.UserId = rating.UserId;

            await _cookbookApiDbContext.SaveChangesAsync();
            return existingRating;
        }

        public async Task RemoveAsync(int id)
        {
            var rating = await _ratingRepository.GetByIdAsync(id);
            if (rating != null)
            {
                await _ratingRepository.RemoveAsync(id);
                await _cookbookApiDbContext.SaveChangesAsync();
            }
        }

        public async Task<(double AverageRating, int Count)> GetRatingStatsByRecipeId(int recipeId)
        {
            var ratings = await _ratingRepository.GetAllAsync();
            var recipeRatings = ratings.Where(r => r.RecipeId == recipeId).ToList();

            if (recipeRatings.Count == 0)
            {
                return (0, 0);
            }

            double average = recipeRatings.Average(r => r.Stars);
            int count = recipeRatings.Count;

            return (average, count);
        }
    }
}
