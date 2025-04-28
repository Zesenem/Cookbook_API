using CookbookAPI.Domain;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Favorite>>> GetAllAsync()
        {
            return await _favoriteService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Favorite>> GetByIdAsync(int id)
        {
            return await _favoriteService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Favorite>> CreateAsync(Favorite favorite)
        {
            return await _favoriteService.CreateAsync(favorite);
        }

        [HttpDelete("user/{userId}/recipe/{recipeId}")]
        public async Task<IActionResult> RemoveFavorite(int userId, int recipeId)
        {
            var result = await _favoriteService.RemoveAsync(userId, recipeId);
            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
