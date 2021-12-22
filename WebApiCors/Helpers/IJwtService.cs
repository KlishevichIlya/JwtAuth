using System.IdentityModel.Tokens.Jwt;

namespace WebApiCors.Helpers
{
    public interface IJwtService
    {
        string Generate(int id);
        JwtSecurityToken Verify(string jwt);
    }
}
