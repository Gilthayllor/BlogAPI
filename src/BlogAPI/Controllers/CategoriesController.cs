using BlogAPI.Data;
using BlogAPI.Extensions;
using BlogAPI.Models;
using BlogAPI.ViewModels;
using BlogAPI.ViewModels.Category;
using BlogAPI.ViewModels.Result;
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
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("01XE07 - Falha interna no servidor."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada."));
                }

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (Exception)
            { 
                return StatusCode(500, new ResultViewModel<Category>("01XE08 - Falha interna no servidor."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EditorCategoryViewModel category) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try
            {
                var newCategory = new Category
                {
                    Name = category.Name
                };

                await _context.Categories.AddAsync(newCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = newCategory.Id }, new ResultViewModel<Category>(newCategory));
            }
            catch(DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>("01XE01 - Não foi possível incluir a categoria."));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<Category>("01XE02 - Falha interna no servidor."));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] EditorCategoryViewModel category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try
            {
                var categoryFound = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (categoryFound == null)
                {
                    return NotFound();
                }

                categoryFound.Name = category.Name;

                _context.Categories.Update(categoryFound);
                await _context.SaveChangesAsync();

                return Ok(category);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>("01XE03 - Não foi possível atualizar a categoria."));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<Category>("01XE04 - Falha interna no servidor."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var categoryFound = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (categoryFound == null)
                {
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada."));
                }

                _context.Categories.Remove(categoryFound);
                await _context.SaveChangesAsync();

                return Ok(categoryFound);
            }
            catch(DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>("01XE05 - Não foi possível excluir a categoria."));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<Category>("01XE06 - Falha interna no servidor."));
            }
        }
    }
}
