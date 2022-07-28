using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly BlogAPIContext _context;

        public CategoriesController(BlogAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if(category == null)
            {
                return NotFound();  
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Category category) 
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id}, category);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Category category)
        {
            var categoryFound = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(categoryFound == null)
            {
                return NotFound();
            }

            categoryFound.Name = category.Name;

            _context.Categories.Update(categoryFound);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var categoryFound = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(categoryFound == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categoryFound);
            await _context.SaveChangesAsync();

            return Ok(categoryFound);
        }
    }
}
