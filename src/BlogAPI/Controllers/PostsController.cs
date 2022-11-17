using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.ViewModels.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public PostsController(BlogAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var posts = await _context.Posts
                .AsNoTracking()
                .Select(x => new
                {
                })
                .ToListAsync();

            return Ok(new ResultViewModel<List<Post>>(posts));
        }
    }
}
