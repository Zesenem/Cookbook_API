using CookbookAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookbookAPI.Services.Interfaces
{
    public interface IRatingService
    {
        Task<List<Rating>> GetAllAsync();

        Task<Rating> GetByIdAsync(int id);

        Task<Rating> CreateAsync(Rating rating);

        Task<Rating> UpdateAsync(Rating rating);

        Task RemoveAsync(int id);

        Task<(double AverageRating, int Count)> GetRatingStatsByRecipeId(int recipeId);
    }
}
