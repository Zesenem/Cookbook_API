using CookbookAPI.Domain;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Rating>>> GetAllAsync()
        {
            return await _ratingService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetByIdAsync(int id)
        {
            return await _ratingService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Rating>> CreateAsync(Rating rating)
        {
            return await _ratingService.CreateAsync(rating);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Rating>> UpdateAsync(Rating rating)
        {
            return await _ratingService.UpdateAsync(rating);
        }

        [HttpGet("recipe/{recipeId}")]
        public async Task<ActionResult<List<Rating>>> GetRatingsByRecipeId(int recipeId)
        {
            var ratings = await _ratingService.GetAllAsync();
            var recipeRatings = ratings.Where(r => r.RecipeId == recipeId).ToList();
            return Ok(recipeRatings);
        }

        [HttpGet("stats/{recipeId}")]
        public async Task<ActionResult<(double AverageRating, int Count)>> GetRatingStatsByRecipeId(int recipeId)
        {
            var (averageRating, count) = await _ratingService.GetRatingStatsByRecipeId(recipeId);
            return Ok(new { AverageRating = averageRating, Count = count });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            await _ratingService.RemoveAsync(id);
            return NoContent();
        }
    }
}
