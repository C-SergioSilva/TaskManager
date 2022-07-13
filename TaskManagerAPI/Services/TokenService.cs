using System.IdentityModel.Tokens.Jwt;
using TaskManagerAPI.Models;

namespace TaskManagerAPI.Services
{
    public class TokenService
    {
        public static string CreateToken(User user) 
        {
            var tokenHandle = new JwtSecurityTokenHandler();
            return  "";
        }
    }
}

