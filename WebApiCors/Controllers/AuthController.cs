using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiCors.Data;
using WebApiCors.Dtos;
using WebApiCors.Entities;
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public IActionResult Login([FromBody] AuthenticateRequest model)
        {
            var response = _jwtService.Authenticate(model, ipAddress());
            if(response is null)
                return BadRequest(new { message = "Incorrect email or password" });
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _jwtService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
                return Unauthorized(new { message = "Invalid token" });
            setTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        [HttpGet("user/{id:int}")]
        [Authorize]
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

        [HttpGet("test")]
        [Authorize]
        public IActionResult Test()
        {
            return Ok("test");
        }

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
