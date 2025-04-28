using CookbookAPI.Domain;
using CookbookAPI.Repositories.Interfaces;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRecipeRepository _recipeRepository;

        public class FavoriteRecipeDto
        {
            public int RecipeId { get; set; }
        }
        public UserController(IUserService userService, IRecipeRepository recipeRepository)
        {
            _userService = userService;
            _recipeRepository = recipeRepository; 
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllAsync()
        {
            return await _userService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetByIdAsync(int id)
        {
            return await _userService.GetByIdAsync(id);
        }

        [HttpGet("{id}/registered")]
        public async Task<ActionResult<User>> GetUserProfileAsync(int id)
        {
            var user = await _userService.GetUserProfileAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("{id}/favorites")]
        public async Task<ActionResult<List<Recipe>>> GetUserFavoritesAsync(int id)
        {
            var favorites = await _userService.GetUserFavoriteRecipesAsync(id);

            if (favorites == null || !favorites.Any())
            {
                return NotFound("No favorite recipes found for this user.");
            }

            return Ok(favorites);
        }

        [HttpPost("{id}/favorites")]
        public async Task<ActionResult> AddFavoriteRecipeAsync(int id, [FromBody] int recipeId)
        {
            if (recipeId <= 0)
            {
                return BadRequest("Recipe ID must be greater than zero.");
            }

            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            try
            {
                await _userService.AddFavoriteRecipeAsync(id, recipeId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> CreateAsync(User user)
        {

            return await _userService.RegisterAsync(user);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateAsync(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }
    
            var existingUser = await _userService.GetByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }
    
            existingUser.Name = user.Name; 
            existingUser.Email = user.Email;

            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = user.Password; 
            }
      
            await _userService.UpdateAsync(existingUser);
            return NoContent(); 
        }

        [Authorize]
        [HttpPut("{id}/block")]
        public async Task<IActionResult> BlockUserAsync(int id)
        {
            await _userService.BlockUserAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}/unblock")]
        public async Task<IActionResult> UnblockUserAsync(int id)
        {
            await _userService.UnblockUserAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            await _userService.RemoveAsync(id);
            return NoContent();
        }
    }
}
