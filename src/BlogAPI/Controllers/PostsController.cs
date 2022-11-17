using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.ViewModels.Posts;
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
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Author = $"{x.Author.Name} ({x.Author.Email})",
                    Category = x.Category.Name,
                    LastUpdateDate = x.UpdatedAt,
                    Title = x.Title
                })
                .ToListAsync();

            return Ok();
        }
    }
}
