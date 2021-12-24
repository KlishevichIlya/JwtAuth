using System.IdentityModel.Tokens.Jwt;
using WebApiCors.Entities;
using WebApiCors.Models;

namespace WebApiCors.Helpers
{
    public interface IJwtService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
        AuthenticateResponse RefreshToken(string token, string ipAddress);       
    }
}
