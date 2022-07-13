using System.IdentityModel.Tokens.Jwt;
using TaskManagerDomain.Entities;

namespace TaskManagerService.Service
{
    public class TokenServices
    {
        public static string CreateToken(User user)
        {
            var tokenHandle = new JwtSecurityTokenHandler();
            return "";
        }
    }
}
