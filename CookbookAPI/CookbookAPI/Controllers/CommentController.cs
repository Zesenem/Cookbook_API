using CookbookAPI.Domain;
using CookbookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{  
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetAllAsync()
        {
            return await _commentService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetByIdAsync(int id)
        {
            return await _commentService.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateAsync(Comment comment)
        {
            return await _commentService.CreateAsync(comment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync(int id)
        {
            await _commentService.RemoveAsync(id);
            return NoContent();
        }
    }
}

