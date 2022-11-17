using BlogAPI.Data;
using BlogAPI.Extensions;
using BlogAPI.Models;
using BlogAPI.Services;
using BlogAPI.ViewModels;
using BlogAPI.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly BlogAPIContext _context;

        public AccountController(TokenService tokenService, BlogAPIContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<List<string>>(ModelState.GetErrors()));
            }

            
            try
            {
                var user = new User
                {
                    Name = registerViewModel.Name,
                    Email = registerViewModel.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(registerViewModel.Password),
                    About = registerViewModel.About,
                    Image = registerViewModel.Image
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    registerViewModel.Password
                }));
            }
            catch (DbUpdateException e)
            {
                return StatusCode(400, new ResultViewModel<string>("02XE01 - Este e-mail já está cadastrado."));
            }
            catch
            {
                return StatusCode(500, "02XE02 - Falha interna no servidor.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<List<string>>(ModelState.GetErrors()));
            }

            var user = await _context.Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == loginViewModel.Email);
            
            if(user == null)
            {
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos."));
            }

            if (!BCrypt.Net.BCrypt.Verify(loginViewModel.Password, user.Password))
            {
                return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos."));
            }

            try
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(new ResultViewModel<string>(token, null));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<string>("02XE03 - Falha interna no servidor."));
            }
        }
    }
}
