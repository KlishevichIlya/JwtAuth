using System.Text.Json.Serialization;
using WebApiCors.Models;

namespace WebApiCors.Entities
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }        
        public string JwtToken { get; set; }

        [JsonIgnore] 
        public string RefreshToken { get; set; }

        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;           
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
