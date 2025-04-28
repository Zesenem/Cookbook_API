using CookbookAPI.Domain;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllAsync()
        {
            return await _categoryService.GetAllAsync();          
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetByIdAsync(int id)
        {
            return await _categoryService.GetByIdAsync(id);                 
        }
        
        [HttpPost]
        public async Task<ActionResult<Category>> CreateAsync(Category category)
        {          
            return await _categoryService.CreateAsync(category);
        }
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            await _categoryService.RemoveAsync(id);
            return NoContent();
        }
    }
}
