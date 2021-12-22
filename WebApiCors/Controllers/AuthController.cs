using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiCors.Data;
using WebApiCors.Dtos;
using WebApiCors.Filters;
using WebApiCors.Helpers;
using WebApiCors.Models;

namespace WebApiCors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public AuthController(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            };
            _userRepository.Create(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _userRepository.GetByEmail(dto.Email);
            if (user is null)
                return BadRequest(new { message = "Invalid credentials" });
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return BadRequest(new { message = " Invalid password" });
            var jwt = _jwtService.Generate(user.Id);
            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
            });
            return Ok(new 
            {
                message = "success"
            });
        }

        [HttpGet("user/{id:int}")]
        [TypeFilter(typeof(AuthorizationAttribute))]
        public IActionResult User(int id)
        {
            try
            {               
                var user = _userRepository.GetById(id);
                return Ok(user);
            }
            catch (Exception)
            {
                return BadRequest("Wrong UserId");
            }            
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok(new
            {
                message = "Success"
            });
        }
    }
}
