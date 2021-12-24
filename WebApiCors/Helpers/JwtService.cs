using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApiCors.Data;
using WebApiCors.Entities;
using WebApiCors.Models;

namespace WebApiCors.Helpers
{
    public class JwtService : IJwtService
    {
        private readonly string secureKey = "this is a very secure key";
        private readonly UserContext _context;
        public JwtService(UserContext context)
        {
            _context = context;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress)
        {            
            var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return null;

            var jwt = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);

            user.RefreshTokens.Add(refreshToken);
            _context.Update(user);
            _context.SaveChanges();
            return new AuthenticateResponse(user, jwt, refreshToken.Token);
        }

        public AuthenticateResponse RefreshToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(x => x.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                return null;

            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == token);
            if(refreshToken.IsExpired)
                return null;

            var newRefreshToken = generateRefreshToken(ipAddress);
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            var jwtToken = generateJwtToken(user);
            return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
        }


        private string generateJwtToken(User user)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);                      
            var payload = new JwtPayload(null, null, new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("userId", user.Id.ToString()),
            }, null, DateTime.UtcNow.AddMinutes(5)) ;
            
            var securityToken = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }       
    }
}
