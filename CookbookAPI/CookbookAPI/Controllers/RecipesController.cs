using CookbookAPI.Domain;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CookbookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly IUserService _userService;

        public RecipesController(IRecipeService recipeService, IUserService userService)
        {
            _recipeService = recipeService;
            _userService = userService;
        }

        private async Task<User> GetCurrentUserAsync()
        {
            var email = User?.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            var user = await _userService.GetByEmailAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            return user;
        }

        [HttpGet]
        public async Task<ActionResult<List<Recipe>>> GetAllAsync()
        {
            var recipes = await _recipeService.GetAllAsync();
            return Ok(recipes);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Recipe>>> SearchRecipesAsync(
            [FromQuery] string? name,
            [FromQuery] string? difficulty,
            [FromQuery] int? categoryId,
            [FromQuery] int? duration)
        {
            var recipes = await _recipeService.SearchRecipesAsync(name, categoryId, difficulty, duration);

            if (recipes == null || recipes.Count == 0)
            {
                return NotFound("No recipes found matching the search criteria.");
            }

            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetByIdAsync(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return recipe;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Recipe>> CreateAsync([FromBody] Recipe recipe)
        {
            if (recipe == null)
            {
                return BadRequest("Recipe cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(recipe.Ingredients))
            {
                return BadRequest("At least one ingredient is required.");
            }

            await _recipeService.CreateAsync(recipe);
            return Ok(recipe);
        }

        [Authorize]
        [HttpPut("{id}/approve")]
        public async Task<ActionResult> ApproveRecipeAsync(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);

            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            await _recipeService.ApproveRecipeAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Recipe>> UpdateAsync(int id, [FromBody] Recipe updatedRecipe)
        {
            var existingRecipe = await _recipeService.GetByIdAsync(id);

            if (existingRecipe == null)
            {
                return NotFound("Recipe not found.");
            }
            existingRecipe.Name = updatedRecipe.Name;
            existingRecipe.Instructions = updatedRecipe.Instructions;
            existingRecipe.Ingredients = updatedRecipe.Ingredients;
            existingRecipe.Duration = updatedRecipe.Duration;
            existingRecipe.Difficulty = updatedRecipe.Difficulty;
            existingRecipe.CategoryId = updatedRecipe.CategoryId;

            var result = await _recipeService.UpdateAsync(existingRecipe);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveAsync(int id)
        {
            await _recipeService.RemoveAsync(id);
            return NoContent();
        }
    }
}
