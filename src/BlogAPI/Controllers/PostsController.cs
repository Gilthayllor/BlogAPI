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
        public async Task<IActionResult> GetAsync([FromQuery] int page = 0, [FromQuery] int pageSize = 25)
        {
            try
            {
                var count = await _context.Posts.AsNoTracking().CountAsync();
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
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("03XE01 - Falha interna no servidor."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostDetailsAsync([FromRoute] int id)
        {
            try
            {
                var post = await _context.Posts
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .Include(x => x.Author)
                    .ThenInclude(x => x.Roles)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (post == null)
                {
                    return NotFound(new ResultViewModel<Post>("Post não encontrado."));
                }

                return Ok(new ResultViewModel<Post>(post));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("03XE02 - Falha interna no servidor."));
            }
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetPostByCategoryAsync([FromRoute] string category, [FromQuery] int page = 0, [FromQuery] int pageSize = 25)
        {
            try
            {
                var count = await _context.Posts.AsNoTracking().CountAsync();
                var posts = await _context.Posts
                    .AsNoTracking()
                    .Include(x => x.Author)
                    .Include(x => x.Category)
                    .Where(x => x.Category.Name == category)
                    .Select(x => new ListPostsViewModel
                    {
                        Id = x.Id,
                        Author = $"{x.Author.Name} ({x.Author.Email})",
                        Category = x.Category.Name,
                        LastUpdateDate = x.UpdatedAt,
                        Title = x.Title
                    })
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .OrderByDescending(x => x.LastUpdateDate)
                    .ToListAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    total = count,
                    page,
                    pageSize,
                    posts
                }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("03XE03 - Falha interna no servidor."));
            }
        }
    }
}
